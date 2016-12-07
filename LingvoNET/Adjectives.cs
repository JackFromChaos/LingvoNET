﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LingvoNET
{
    /// <summary>
    /// Словарь прилагательных
    /// </summary>
    public static class Adjectives
    {
        private readonly static List<AdjectiveRaw> items = new List<AdjectiveRaw>();
        internal readonly static Schemas schemas = new Schemas();

        static Adjectives()
        {
            schemas.BeginInit();
            items.Clear();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "LingvoNET.Dict.прилаг.bin";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var zip = new GZipStream(stream, CompressionMode.Decompress))
            using (var sr = new StreamReader(zip, Encoding.GetEncoding(1251)))
            while (sr.Peek() >= 0)
            {
                var line = sr.ReadLine();
                if (!string.IsNullOrEmpty(line))
                    items.Add(ParseAdjective(line));
            }

            schemas.EndInit();
        }

        static AdjectiveRaw ParseAdjective(string line)
        {
            var parts = line.Split('\t');

            var wordStr = parts[0];
            var schemaStr = parts[1];

            var res = new AdjectiveRaw();
            res.Word = wordStr;
            res.SchemaIndex = schemas.GetOrAddSchemaId(schemaStr);

            return res;
        }

        /// <summary>
        /// Поиск по точному или приблизительному совпадению
        /// </summary>
        public static Adjective FindSimilar(string sourceForm, Comparability comparability = Comparability.Undefined)
        {
            var searchWord = PrepareWord(sourceForm);

            var res = items.FindSimilar(new AdjectiveRaw() { Word = searchWord }, new StringReverseComparer<AdjectiveRaw>(), PrepareFilter(comparability));
            if (res.Word == null)
                return null;
            return new Adjective { Word = sourceForm, SchemaIndex = res.SchemaIndex, Inexact = res.Word != searchWord };
        }

        /// <summary>
        /// Поиск по точному или приблизительному совпадению
        /// </summary>
        public static Adjective FindSimilar(string sourceForm, Predicate<Adjective> filter)
        {
            var searchWord = PrepareWord(sourceForm);

            var res = items.FindSimilar(new AdjectiveRaw() { Word = searchWord }, new StringReverseComparer<AdjectiveRaw>(), (item) => filter(new Adjective(item, item.Word)));
            if (res.Word == null)
                return null;
            return new Adjective { Word = sourceForm, SchemaIndex = res.SchemaIndex, Inexact = res.Word != searchWord };
        }

        /// <summary>
        /// Поиск одного точного совпадения. Null - если не найдено.
        /// </summary>
        public static Adjective FindOne(string sourceForm, Comparability comparability = Comparability.Undefined)
        {
            var searchWord = PrepareWord(sourceForm);

            var res = items.FindOne(new AdjectiveRaw() { Word = searchWord }, new StringReverseComparer<AdjectiveRaw>(), PrepareFilter(comparability));
            if (res.Word == null)
                return null;
            return new Adjective(res, sourceForm);
        }

        /// <summary>
        /// Поиск всех точных совпадений(омонимов).
        /// </summary>
        public static IEnumerable<Adjective> FindAll(string sourceForm)
        {
            var searchWord = PrepareWord(sourceForm);

            foreach (var res in items.FindAll(new AdjectiveRaw() { Word = searchWord }, new StringReverseComparer<AdjectiveRaw>()))
                yield return new Adjective(res, sourceForm);
        }

        private static Predicate<AdjectiveRaw> PrepareFilter(Comparability comp)
        {
            if (comp == Comparability.Undefined) return (item) => true;
            return (item) => item.Comparability == comp;
        }

        private static string PrepareWord(string sourceForm)
        {
            var searchWord = sourceForm.ToLowerInvariant();
            return searchWord;
        }

        /// <summary>
        /// Возвращает все слова
        /// </summary>
        public static IEnumerable<Adjective> GetAll()
        {
            foreach (var raw in items)
                yield return new Adjective(raw, raw.Word);
        }
    }

    internal struct AdjectiveRaw
    {
        public string Word;
        public int SchemaIndex;

        public Comparability Comparability
        {
            get
            {
                if(Adjectives.schemas[SchemaIndex].GetForm(Word, 32) != null)
                    return Comparability.Comparable;
                else
                    return Comparability.Incomparable;
            }
        }

        public override string ToString()
        {
            return Word;
        }
    }

    /// <summary>
    /// Прилагательное и его словоформы
    /// </summary>
    public class Adjective
    {
        internal int SchemaIndex;

        /// <summary>
        /// Исходная форма
        /// </summary>
        public string Word { get; internal set; }
        /// <summary>
        /// Результат не точен и был получен по похожему слову
        /// </summary>
        public bool Inexact { get; internal set; }

        /// <summary>
        /// Разряд прилагательного
        /// </summary>
        public Comparability Comparability
        {
            get
            {
                if (Adjectives.schemas[SchemaIndex].GetForm(Word, 32) != null)
                    return Comparability.Comparable;
                else
                    return Comparability.Incomparable;
            }
        }

        internal Adjective(AdjectiveRaw raw, string word)
        {
            this.Word = word;
            if (raw.Word != null)
            {
                SchemaIndex = raw.SchemaIndex;
            }
        }

        public Adjective()
        {
        }

        /// <summary>
        /// Словоформы по падежам, родам и числам
        /// </summary>
        public string this[Case @case, Gender gender]
        {
            get
            {
                var i = @case.IndexWithAnimate(gender);
                i += 8*(int) gender.Gen();

                return Adjectives.schemas[SchemaIndex].GetForm(Word, i);
            }
        }

        /// <summary>
        /// Сравнительные степени прилагательного
        /// </summary>
        public string this[Comparison comparsion]
        {
            get
            {
                if ((int)comparsion > 3)
                    return "";

                var res = Adjectives.schemas[SchemaIndex].GetForm(Word, 32 + (int)comparsion / 2);
                if (string.IsNullOrEmpty(res))
                    return "";

                switch(comparsion)
                {
                    case Comparison.Comparative2:
                    case Comparison.Comparative4:
                        return Char.IsUpper(res[0]) ? "По" + res.ToLowerInvariant() : "по" + res;
                    default:
                        return res;
                }
            }
        }
    }
}
