using DataContract;
using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlDatabase
{
    public class UserMapReduceIndex : AbstractIndexCreationTask<User>
    {
        public UserMapReduceIndex()
        {
            Map = users => from user in users
                           select new
                           {
                               user.UserId,
                               user.Username
                           };
        }
    }

    public class CityMapReduceIndex : AbstractIndexCreationTask<City, ReduceCity>
    {
        public CityMapReduceIndex()
        {
            Map = cities => from city in cities
                            select new
                            {
                                Count = 1,
                                Summery = new[] { new SummeryCity { Name = city.Name, Population = city.Population, Province = city.Province } }
                            };
            Reduce = cities => from city in cities
                               group city by new { city.CountryID } into g
                               select new
                               {
                                   Count = g.Sum(x => x.Count),
                                   Summery = from country in g.SelectMany(x => x.Summery)
                                             select new SummeryCity { Name = g.First().Name, Population = g.First().Population, Province = g.First().Province }
                               };
        }
    }
}
