using System;
using ConcordiaStation.Data.Dto;
using ConcordiaStation.Data.Models;
using ConcordiaStation.Data.Repositories.Interfaces;

namespace ConcordiaStation.SyncApp.Endpoints.Gateways
{
	public class DatabaseGateway : IDatabaseGateway
	{
        private readonly IRepositoryExperiment _repositoryExperiment;
        private readonly IRepositoryPhase _repositoryPhase;
        private readonly IRepositoryScientist _repositoryScientist;
        private readonly IRepositoryComment _repositoryComment;

        public DatabaseGateway(IRepositoryExperiment repositoryExperiment, IRepositoryPhase repositoryPhase, IRepositoryScientist repositoryScientist, IRepositoryComment repositoryComment)
        {
            _repositoryExperiment = repositoryExperiment;
            _repositoryPhase = repositoryPhase;
            _repositoryScientist = repositoryScientist;
            _repositoryComment = repositoryComment;
        }

        public void UpdateExperimentsWithIdTrello(List<ExperimentDto> experimentDtoWithIdTrello)
        {
            foreach (var experimentDto in experimentDtoWithIdTrello)
            {
                var experiment = new Experiment
                {
                    Id = experimentDto.Id,
                    IdListTrello = experimentDto.IdListTrello,
                    Title = experimentDto.Title,
                    Phases = experimentDto.PhasesDto.Select(x => new Phase
                    {
                        Id = x.Id,
                        Title = x.Title,
                        DateOfCreation = x.DateOfCreation,
                        Deadline = x.Deadline,
                        LastActivity = x.LastActivity,
                        Description = x.Description,
                        IdCardTrello = x.IdPhaseInTrello,
                        Priority = x.Priority,
                        Status = x.Status
                    }).ToList()
                };

                _repositoryExperiment.Update(experiment);
                experiment.Phases.Select(x => _repositoryPhase.Update(x));
            }
        }

        public void UpdateDatabaseWithTrelloContent(List<ExperimentDto> experimentDtoFromTrello)
        {
            var comments = _repositoryComment.GetAll();
            var scientists = _repositoryScientist.GetAll();
            var phases = _repositoryPhase.GetAll();
            var experiments = _repositoryExperiment.GetAll();

            foreach (var experimentDto in experimentDtoFromTrello)
            {
                var experiment = experiments.SingleOrDefault(x => x.Id == experimentDto.Id || x.IdListTrello == experimentDto.IdListTrello);
                if (experiment != null)
                {
                    _repositoryExperiment.Update(new Experiment
                    {
                        Id = experimentDto.Id,
                        IdListTrello = experimentDto.IdListTrello,
                        Title = experimentDto.Title,
                        Phases = GetPhasesToUpdate(experimentDto.PhasesDto,phases)
                    });
                }
                else
                {
                    _repositoryExperiment.Insert(new Experiment
                    {
                        IdListTrello = experimentDto.IdListTrello,
                        Title = experimentDto.Title,
                        Phases = GetPhasesToAssociateInExperiment(experimentDto.PhasesDto)
                    });
                }
            }
        }

        private List<Phase> GetPhasesToUpdate(List<PhaseDto> phasesDto, IEnumerable<Phase> phases)
        {
            var listPhase = new List<Phase>();

            foreach (var phaseDto in phasesDto)
            {
                var existingPhase = phases.SingleOrDefault(x => x.Id == phaseDto.Id || x.IdCardTrello == phaseDto.IdPhaseInTrello);
                if (existingPhase != null)
                {
                    var phase = new Phase()
                    {
                        Id = existingPhase.Id,
                        IdCardTrello = phaseDto.IdPhaseInTrello,
                        Deadline = phaseDto.Deadline,
                        Description = phaseDto.Description,
                        LastActivity = phaseDto.LastActivity,
                        DateOfCreation = phaseDto.DateOfCreation,
                        Priority = phaseDto.Priority,
                        Status = phaseDto.Status,
                        Title = phaseDto.Title,
                        Scientists = GetScientist(phaseDto.Scientists),
                        Comments = GetComments(phaseDto.Comments),
                        Experiment = _repositoryExperiment.GetById(phaseDto.Eperiment.Id)
                    };

                    //_repositoryPhase.Update(phase);
                    listPhase.Add(phase);
                }
                else
                {
                    var phase = new Phase
                    {
                        IdCardTrello = phaseDto.IdPhaseInTrello,
                        Deadline = phaseDto.Deadline,
                        Description = phaseDto.Description,
                        LastActivity = phaseDto.LastActivity,
                        DateOfCreation = phaseDto.DateOfCreation,
                        Priority = phaseDto.Priority,
                        Status = phaseDto.Status,
                        Title = phaseDto.Title,
                        Scientists = GetScientist(phaseDto.Scientists),
                        Comments = GetComments(phaseDto.Comments),
                        Experiment = _repositoryExperiment.GetById(phaseDto.Eperiment.Id)
                    };

                    _repositoryPhase.Insert(phase);
                    listPhase.Add(phase);
                }
            }

            return listPhase;
        }

        private List<Comment> GetComments(List<CommentDto> commentsDto)
        {
            var listComment = new List<Comment>();

            foreach (var commentDto in commentsDto)
            {
                var comment = _repositoryComment.GetById(commentDto.Id);
                if (comment != null)
                {
                    listComment.Add(comment);
                }
                else
                {
                    var commentToAdd = new Comment
                    {
                        Content = commentDto.Content,
                        IdPhaseTrello = commentDto.IdPhaseTrello,
                        PublicationDate = commentDto.PublicationDate,
                        Phase = _repositoryPhase.GetById(commentDto.Phase.Id),
                        Scientist = _repositoryScientist.GetById(commentDto.Scientist.Id)
                    };

                    commentToAdd = _repositoryComment.Insert(commentToAdd);
                    listComment.Add(commentToAdd);
                }
            }

            return listComment;
        }

        private List<Scientist> GetScientist(List<ScientistDto> scientistsDto)
        {
            var listScientist = new List<Scientist>();

            foreach (var scientistDto in scientistsDto)
            {
                var scientist = _repositoryScientist.GetById(scientistDto.Id);
                if (scientist != null)
                {
                    listScientist.Add(scientist);
                }
                else
                {
                    var scientistToInsert = new Scientist
                    {
                        FamilyName = scientistDto.FamilyName,
                        GivenName = scientistDto.GivenName,
                        Phase = scientistDto.Phases.Select(x => _repositoryPhase.GetById(x.Id)).FirstOrDefault()
                    };
                    scientistToInsert = _repositoryScientist.Insert(scientistToInsert);
                    listScientist.Add(scientistToInsert);
                }
            }

            return listScientist;
        }

        private List<Phase> GetPhasesToAssociateInExperiment(List<PhaseDto> phasesDto)
        {
            var listPhase = new List<Phase>();

            foreach (var phaseDto in phasesDto)
            {
                var experiment = _repositoryExperiment.GetById(phaseDto.Eperiment.Id);
                var existingPhase = _repositoryPhase.GetById(phaseDto.Id);

                if (existingPhase != null)
                {
                    if (existingPhase.Experiment.Id != experiment.Id)
                    {
                        existingPhase.Experiment = experiment;
                        //_repositoryPhase.Update(existingPhase);
                    }
                    listPhase.Add(existingPhase);
                }
                else
                {
                    var phase = new Phase
                    {
                        IdCardTrello = phaseDto.IdPhaseInTrello,
                        Deadline = phaseDto.Deadline,
                        Description = phaseDto.Description,
                        LastActivity = phaseDto.LastActivity,
                        DateOfCreation = phaseDto.DateOfCreation,
                        Priority = phaseDto.Priority,
                        Status = phaseDto.Status,
                        Title = phaseDto.Title,
                        Scientists = GetScientist(phaseDto.Scientists),
                        Comments = GetComments(phaseDto.Comments),
                        Experiment = experiment //!= null ? experiment : new Experiment { IdListTrello = phaseDto.Eperiment.IdListTrello, Title = phaseDto.Eperiment.Title }
                    };

                    _repositoryPhase.Insert(phase);
                    listPhase.Add(phase);
                }
            }

            return listPhase;
        }


        public List<ExperimentDto> GetExperimentDtoFromLocalDb()
        {
            var experimentsDto = new List<ExperimentDto>();

            var comments = _repositoryComment.GetAll();
            var scientists = _repositoryScientist.GetAll();
            var phases = _repositoryPhase.GetAll();
            var experiments = _repositoryExperiment.GetAll();

            foreach (var experiment in experiments)
            {
                var experimentDto = new ExperimentDto
                {
                    Id = (int)experiment.Id,
                    IdListTrello = experiment.IdListTrello,
                    Title = experiment.Title,
                    PhasesDto = MakePhaseDto(experiment.Phases,experiment)
                };

                experimentsDto.Add(experimentDto);
            }

            return experimentsDto;
        }

        private List<PhaseDto> MakePhaseDto(List<Phase> phases, Experiment experiment)
        {
            var phasesDtos = new List<PhaseDto>();

            foreach (var phase in phases)
            {
                var phaseDto = new PhaseDto
                {
                    Id = (int)phase.Id,
                    Title = phase.Title,
                    Description = phase.Description,
                    Status = phase.Status,
                    Priority = phase.Priority,
                    DateOfCreation = phase.DateOfCreation,
                    Deadline = phase.Deadline,
                    LastActivity = phase.LastActivity,
                    IdPhaseInTrello = phase.IdCardTrello,
                    Scientists = MakeScinetistDto(phase.Scientists),
                    Comments = MakeCommentDto(phase.Comments),
                };

                phasesDtos.Add(phaseDto);
            }

            return phasesDtos;
        }

        private List<ScientistDto> MakeScinetistDto(List<Scientist> scientists)
        {
            var scientistsDto = new List<ScientistDto>();

            foreach (var scientst in scientists)
            {
                var scientistDto = new ScientistDto
                {
                    GivenName = scientst.GivenName,
                    FamilyName = scientst.FamilyName,
                    IdTrello = scientst.IdTrello,
                    Id = (int)scientst.Id
                };

                scientistsDto.Add(scientistDto);
            }

            return scientistsDto;
        }

        private List<CommentDto> MakeCommentDto(List<Comment> comments)
        {
            var commentsDto = new List<CommentDto>();

            foreach (var comment in comments)
            {
                var scientist = _repositoryScientist.GetById((int)comment.Scientist.Id);
                var scientistDto = new ScientistDto
                {
                    Id = (int)scientist.Id,
                    FamilyName = scientist.FamilyName,
                    GivenName = scientist.GivenName,
                    IdTrello = scientist.IdTrello
                };

                var commentDto = new CommentDto
                {
                    Content = comment.Content,
                    Id = (int)comment.Id,
                    IdPhaseTrello = comment.IdPhaseTrello,
                    PublicationDate = comment.PublicationDate,
                    Scientist = scientistDto
                };

                commentsDto.Add(commentDto);
            }

            return commentsDto;
        }

    }
}

