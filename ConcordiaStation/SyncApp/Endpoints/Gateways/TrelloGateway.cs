using System;
using ConcordiaStation.Data.Dto;
using ConcordiaStation.Data.Enum;
using Microsoft.Extensions.Configuration;
using TrelloDotNet;
using TrelloDotNet.Model;

namespace ConcordiaStation.SyncApp.Endpoints.Gateways
{
	public class TrelloGateway : ITrelloGateway
	{
        private readonly TrelloClient _trelloClient;
        private readonly IConfiguration _configuration;
        private readonly string? _apiKey;
        private readonly string? _token;

        public TrelloGateway(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiKey = _configuration.GetSection("TrelloConnection:appKey").Value;
            _token= _configuration.GetSection("TrelloConnection:token").Value;
            _trelloClient = new TrelloClient(_apiKey, _token);
        }

        public async Task<List<ExperimentDto>> GetExperimentDtoFromTrelloBoard(string boardId)
        {
            var listExperimentDto = new List<ExperimentDto>();
            var listsTrello = await _trelloClient.GetListsOnBoardAsync(boardId);

            foreach (var list in listsTrello)
            {
                var cards = await _trelloClient.GetCardsInListAsync(list.Id);
                var experimentDto = new ExperimentDto { IdListTrello = list.Id, Title = list.Name, PhasesDto = await GetNewPhaseDto(cards, list.Id) };
                listExperimentDto.Add(experimentDto);
            }

            return listExperimentDto;
        }

        public async Task<List<ExperimentDto>> UpdateTrelloBoardWithLocalDbContentAsync(List<ExperimentDto> experimentDtoFromLocalDb, string boardId)
        {
            foreach (var experimentDto in experimentDtoFromLocalDb)
            {
                try
                {
                    var listTrello = await _trelloClient.GetListAsync(experimentDto.IdListTrello);
                    if (listTrello != null)
                    {
                        var cards = await _trelloClient.GetCardsInListAsync(listTrello.Id);
                        var phaseDto = experimentDto.PhasesDto;

                        foreach (var card in cards)
                        {
                            var phase = phaseDto.Where(x => x.IdPhaseInTrello == card.Id).First();
                            var commentsCard = await _trelloClient.GetAllCommentsOnCardAsync(card.Id);
                            var commentsToAdd = phaseDto
                                                .SelectMany(x => x.Comments)
                                                .Where(comment => !commentsCard.Any(commentsCard => commentsCard.Data.Text == comment.Content));
                            commentsToAdd.Select(async x => await _trelloClient.AddCommentAsync(card.Id, new Comment { Text = x.Content }));
                        }

                        experimentDtoFromLocalDb.Remove(experimentDto);
                    }
                    else
                    {
                        listTrello = new List
                        {
                            BoardId = boardId,
                            Name = experimentDto.Title,
                        };
                        listTrello = await _trelloClient.AddListAsync(listTrello);
                        experimentDto.IdListTrello = listTrello.Id;
                        foreach (var phase in experimentDto.PhasesDto)
                        {
                            var card = new Card
                            {
                                ListId = listTrello.Id,
                                Name = phase.Title,
                                Description = phase.Description,
                                MemberIds = phase.Scientists.Select(x => x.IdTrello).ToList(),
                                BoardId = boardId,
                                Due = phase.Deadline,
                            };
                            var cardWithIdTrello = await _trelloClient.AddCardAsync(card);
                            experimentDto.PhasesDto.Where(x => x.Id == phase.Id).First().IdPhaseInTrello = cardWithIdTrello.Id;
                            foreach (var comment in phase.Comments)
                            {
                                var commentTrello = new TrelloDotNet.Model.Comment
                                {
                                    Text = comment.Content
                                };
                                var commentTrelloInPhase = await _trelloClient.AddCommentAsync(cardWithIdTrello.Id, commentTrello);
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    await Console.Out.WriteLineAsync(e.Message);
                }
            }

            return experimentDtoFromLocalDb;
        }

        private async Task<List<PhaseDto>> GetNewPhaseDto(List<Card> cards, string idList)
        {
            var listScientistDto = new List<ScientistDto>();
            var listComments = new List<CommentDto>();
            var listPhaseDto = new List<PhaseDto>();

            foreach (var card in cards)
            {
                var members = await _trelloClient.GetMembersOfCardAsync(card.Id);
                foreach (var member in members)
                {
                    var names = member.FullName.Split(" ");
                    var scientistsDto = new ScientistDto
                    {
                        GivenName = names[0],
                        FamilyName = names[1],
                        IdTrello = member.Id
                    };

                    listScientistDto.Add(scientistsDto);
                }
                var comments = await _trelloClient.GetAllCommentsOnCardAsync(card.Id);
                foreach (var comment in comments)
                {
                    var name = comment.MemberCreator.FullName.Split(" ");
                    var commentDto = new CommentDto
                    {
                        IdPhaseTrello = comment.Data.Card.Id,
                        Content = comment.Data.Text,
                        IdCommentTrello = comment.Id,
                        PublicationDate = comment.Date.DateTime,
                        Scientist = new ScientistDto { IdTrello = comment.Id, GivenName = name.First(),FamilyName = name.Last()},
                        Phase = new PhaseDto { IdPhaseInTrello = comment.Data.Card.Id}
                    };
                    listComments.Add(commentDto);
                }
                var status = _trelloClient.GetActionsOnCardAsync(card.Id).Status;
                var due = card.Due.HasValue ? card.Due.Value.DateTime : DateTime.MaxValue;
                var dateOfCreation = card.Created.HasValue ? card.Created.Value.DateTime : DateTime.Now;

                var myPriority = card.Labels.FirstOrDefault() == null ? Data.Enum.Priority.NotDefined : card.Labels.FirstOrDefault().Name switch
                {
                    "LowPriority" => Data.Enum.Priority.LowPriority,
                    "MediumPriority" => Data.Enum.Priority.MediumPriority,
                    "HighPriority" => Data.Enum.Priority.HighPriority,
                    "RunningOutOfTime" => Data.Enum.Priority.RunningOutOfTime,
                    _ => Data.Enum.Priority.NotDefined
                };

                var phasesDto = new PhaseDto
                {
                    IdPhaseInTrello = card.Id,
                    Description = card.Description,
                    LastActivity = card.LastActivity.DateTime,
                    Deadline = due,
                    DateOfCreation = dateOfCreation,
                    Priority = myPriority,
                    Eperiment = new ExperimentDto { IdListTrello = idList },
                    Scientists = listScientistDto,
                    Status = (Data.Enum.Status)status,
                    Title = card.Name,
                    Comments = listComments
                };
                listPhaseDto.Add(phasesDto);
            }
            return listPhaseDto;
        }

    }
}

