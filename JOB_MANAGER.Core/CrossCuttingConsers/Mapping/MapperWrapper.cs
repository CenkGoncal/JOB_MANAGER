using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace JOB_MANAGER.CrossCuttingConsers.Mapping
{
    public static class MapperWrapper
    {
        public static IEnumerable<TDest> Map<TSource, TDest>(IEnumerable<TSource> sourceList)
           where TSource : class
           where TDest : class
        {
            var config = new MapperConfiguration(f => f.CreateMap<IEnumerable<TSource>, IEnumerable<TDest>>());
            var mapper = new Mapper(config);

            IEnumerable<TDest> destList = mapper.Map<IEnumerable<TDest>>(sourceList);

            return destList;
        }

        public static List<TDest> Map<TSource, TDest>(List<TSource> sourceList)
            where TSource : class
            where TDest : class
        {
            var config = new MapperConfiguration(f => f.CreateMap<List<TSource>, List<TDest>>());
            var mapper = new Mapper(config);

            List<TDest> destList = mapper.Map<List<TDest>>(sourceList);

            return destList;
        }

        public static TDest Map<TSource, TDest>(TSource source, TDest destination)
            where TSource : class
            where TDest : class
        {

            var config = new MapperConfiguration(f => f.CreateMap<TSource, TDest>());
            var mapper = new Mapper(config);

            TDest dest = mapper.Map<TSource, TDest>(source, destination);

            return dest;
        }


        public static TDest Map<TSource, TDest>(TSource source)
            where TSource : class
            where TDest : class
        {

            var config = new MapperConfiguration(f => f.CreateMap<TSource, TDest>());
            var mapper = new Mapper(config);

            TDest dest = mapper.Map<TSource, TDest>(source);

            return dest;
        }
    }
}
