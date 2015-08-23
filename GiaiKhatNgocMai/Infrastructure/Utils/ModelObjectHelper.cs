using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Infrastructure.Utils
{
    public class ModelObjectHelper
    {
        public static TDestination CopyObject<TSource, TDestination>(TSource source, TDestination destination) 
               where TSource : class
               where TDestination : class
        {
            var sourceProps = source.GetType().GetProperties().OrderBy(i => i.Name).ToList();
            var destinationProps = destination.GetType().GetProperties().OrderBy(i => i.Name).ToList();
            foreach (var prop in sourceProps)
            {
                var destProp = destinationProps.SingleOrDefault(i => i.Name == prop.Name && i.PropertyType == prop.PropertyType);
                if (destProp == null) continue;
                destProp.SetValue(destination, prop.GetValue(source));
            }
            return destination;
        }
    }
}