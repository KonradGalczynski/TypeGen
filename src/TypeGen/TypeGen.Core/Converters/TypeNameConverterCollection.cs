﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeGen.Core.Converters
{
    /// <summary>
    /// Represents a collection of type name converters
    /// </summary>
    public class TypeNameConverterCollection : IEnumerable<ITypeNameConverter>
    {
        private readonly IList<ITypeNameConverter> _converters;

        public TypeNameConverterCollection()
        {
            _converters = new List<ITypeNameConverter> { new NoChangeConverter() };
        }

        public TypeNameConverterCollection(IEnumerable<ITypeNameConverter> converters)
        {
            _converters = converters.ToList();
            _converters.Insert(0, new NoChangeConverter());
        }

        /// <summary>
        /// Adds a type converter to the collection
        /// </summary>
        /// <param name="converter"></param>
        public void Add(ITypeNameConverter converter)
        {
            _converters.Add(converter);
        }

        /// <summary>
        /// Removes a type converter from the collection.
        /// Throws an exception if attempted to remove the first, default NoChangeConverter converter.
        /// </summary>
        /// <param name="converter"></param>
        public void Remove(ITypeNameConverter converter)
        {
            if (_converters.IndexOf(converter) == 0)
            {
                throw new ApplicationException("Cannot remove the first, default NoChangeConverter");
            }

            _converters.Remove(converter);
        }

        /// <summary>
        /// Converts a type name using the chain of converters
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string Convert(string name, Type type)
        {
            return _converters.Aggregate(name, (current, converter) => converter.Convert(current, type));
        }

        public IEnumerator<ITypeNameConverter> GetEnumerator()
        {
            return _converters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
