using api_places.Models.Places;
using Library.Context.Places.States;
using Library.Models.Places;
using System.Collections.Generic;

namespace api_places.Persistence
{
    public class StatesPersistence
    {
        private readonly ClassStates classStates;
        public StatesPersistence()
        {
            classStates = new ClassStates();
        }

        public IEnumerable<States> List()
        {
            var listStates = new List<States>();
            var allStates = classStates.List();

            if (allStates != null)
            {
                foreach (var states in allStates)
                {
                    var flag = new Flag()
                    {
                        Id = states.Flag.Id,
                        Symbol = states.Flag.Symbol,
                        Path = states.Flag.Path
                    };
                    var state = new States()
                    {
                        Id = states.Id,
                        Label = states.Label,
                        CountryId = states.CountryId,
                        Flag = flag
                    };

                    listStates.Add(state);
                }

                return listStates;
            }
            else
            {
                return null;
            }
        }
        public States Get(int? Id)
        {
            var states = classStates.Get(Id);
            
            if (states != null)
            {
                var state = new States()
                {
                    Id = states.Id,
                    Label = states.Label,
                    Flag = new Flag()
                    {
                        Id = states.Flag.Id,
                        Symbol = states.Flag.Symbol,
                        Path = states.Flag.Path
                    },
                    CountryId = states.CountryId
                };
                return state;
            }
            else
            {
                return null;
            }
        }
        public void Post(States states)
        {
            var statesDomain = new StateDomain()
            {
                Id = states.Id,
                Label = states.Label,
                Flag = new FlagDomain()
                {
                    Id = states.Flag.Id,
                    Symbol = states.Flag.Symbol,
                    Path = states.Flag.Path
                },
                CountryId = states.CountryId
            };

            classStates.Post(statesDomain);
        }
        public void Put(States states, int? Id)
        {
            var statesDomain = new StateDomain()
            {
                Id = states.Id,
                Label = states.Label,
                Flag = new FlagDomain()
                {
                    Id = states.Flag.Id,
                    Symbol = states.Flag.Symbol,
                    Path = states.Flag.Path
                },
                CountryId = states.CountryId,
            };

            classStates.Put(statesDomain, Id);
        }
        public void Delete(int? Id)
        {
            classStates.Delete(Id);
        }
    }
}