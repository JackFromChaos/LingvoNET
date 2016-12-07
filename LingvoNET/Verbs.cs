using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LingvoNET
{
    /// <summary>
    /// Словарь глаголов
    /// </summary>
    public static class Verbs
    {
        private readonly static List<VerbRaw> items = new List<VerbRaw>();
        internal readonly static Schemas schemas = new Schemas();
        private readonly static List<Pair> imperfectToPerfect = new List<Pair>();
        public static event EventHandler<CustomWordsNeededEventArgs> CustomWordsNeeded;

        /// <summary>
        /// Глагол 'быть'
        /// </summary>
        public static Verb ToBe { get; private set; }

        private static bool Initialized;

        public static void Init()
        {
            Initialized = true;

            schemas.BeginInit();
            items.Clear();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "LingvoNET.Dict.глаголы.bin";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var zip = new GZipStream(stream, CompressionMode.Decompress))
            using (var sr = new StreamReader(zip, Encoding.GetEncoding(1251)))
            while (sr.Peek() >= 0)
            {
                var line = sr.ReadLine();
                if (!string.IsNullOrEmpty(line))
                    items.Add(ParseVerb(line));
            }

            //additional custom words
            if (CustomWordsNeeded != null)
            {
                var ea = new CustomWordsNeededEventArgs() { CustomWords = new List<string>() };
                CustomWordsNeeded(null, ea);

                var comparer = new StringReverseComparer<VerbRaw>();

                foreach(var line in ea.CustomWords)
                if (!string.IsNullOrEmpty(line))
                {
                    var v = ParseVerb(line);

                    var i = items.BinarySearch(v, comparer);
                    if (i >= 0 && i < items.Count)
                    {
                        items[i] = v;
                    }
                    else
                    {
                        i = -i - 1;
                        items.Insert(i, v);
                    }
                }
            }

            schemas.EndInit();

            //загружаем пары несоверш-соверш
            resourceName = "LingvoNET.Dict.ImperfectPerfect.bin";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var zip = new GZipStream(stream, CompressionMode.Decompress))
            using (var sr = new StreamReader(zip, Encoding.GetEncoding(1251)))
                while (sr.Peek() >= 0)
                {
                    var line = sr.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        var parts = line.Split('\t');
                        imperfectToPerfect.Add(new Pair{Item1 = parts[0], Item2 = parts[1]});
                    }
                }

            //
            ToBe = FindOne("быть");
        }

        static VerbRaw ParseVerb(string line)
        {
            var parts = line.Split('\t');

            var wordStr = parts[0];
            var aspectStr = parts[1];
            var schemaStr = parts[2];

            var res = new VerbRaw();
            res.Word = wordStr;
            res.SchemaIndex = schemas.GetOrAddSchemaId(schemaStr);
            switch(aspectStr)
            {
                case "нсв": res.Aspect = VerbAspect.Imperfect; break;
                case "св": res.Aspect = VerbAspect.Perfect; break;
                case "св-нсв": res.Aspect = VerbAspect.PerfectImperfect; break;
            }

            return res;
        }

        /// <summary>
        /// Поиск по точному или приблизительному совпадению
        /// </summary>
        public static Verb FindSimilar(string sourceForm, VerbAspect aspect = VerbAspect.Undefined)
        {
            var searchWord = PrepareWord(sourceForm);

            var res = items.FindSimilar(new VerbRaw() { Word = searchWord }, new StringReverseComparer<VerbRaw>(), PrepareFilter(aspect));
            if (res.Word == null)
                return null;
            return new Verb { Word = sourceForm, SchemaIndex = res.SchemaIndex, Aspect = res.Aspect, Inexact = res.Word != searchWord };
        }

        /// <summary>
        /// Поиск по точному или приблизительному совпадению
        /// </summary>
        public static Verb FindSimilar(string sourceForm, Predicate<Verb> filter)
        {
            var searchWord = PrepareWord(sourceForm);

            var res = items.FindSimilar(new VerbRaw() { Word = searchWord }, new StringReverseComparer<VerbRaw>(), (item) => filter(new Verb(item, item.Word)));
            if (res.Word == null)
                return null;
            return new Verb { Word = sourceForm, SchemaIndex = res.SchemaIndex, Aspect = res.Aspect, Inexact = res.Word != searchWord };
        }

        /// <summary>
        /// Поиск одного точного совпадения. Null - если не найдено.
        /// </summary>
        public static Verb FindOne(string sourceForm, VerbAspect aspect = VerbAspect.Undefined)
        {
            var searchWord = PrepareWord(sourceForm);

            var res = items.FindOne(new VerbRaw() { Word = searchWord }, new StringReverseComparer<VerbRaw>(), PrepareFilter(aspect));
            if (res.Word == null)
                return null;
            return new Verb(res, sourceForm);
        }

        /// <summary>
        /// Поиск всех точных совпадений(омонимов).
        /// </summary>
        public static IEnumerable<Verb> FindAll(string sourceForm)
        {
            var searchWord = PrepareWord(sourceForm);

            foreach (var res in items.FindAll(new VerbRaw() { Word = searchWord }, new StringReverseComparer<VerbRaw>()))
                yield return new Verb(res, sourceForm);
        }

        public static IEnumerable<Verb> FindPerfects(string sourceFormImperfect)
        {
            var searchWord = PrepareWord(sourceFormImperfect);

            var i = imperfectToPerfect.BinarySearch(new Pair {Item1 = searchWord});

            if(i >= 0)
            {
                while (imperfectToPerfect[i].Item1 == searchWord)
                    i--;
                i++;
                while (imperfectToPerfect[i].Item1 == searchWord)
                {
                    var perf = imperfectToPerfect[i].Item2;
                    foreach (var res in FindAll(perf.ToUpper(sourceFormImperfect)))
                        yield return res;
                    i++;
                }
            }
        }

        private static Predicate<VerbRaw> PrepareFilter(VerbAspect aspect)
        {
            if (aspect == VerbAspect.Undefined) return (item) => true;
            return (item) => item.Aspect == aspect;
        }

        private static string PrepareWord(string sourceForm)
        {
            if (!Initialized)
                Init();

            var searchWord = sourceForm.ToLowerInvariant();
            return searchWord;
        }

        /// <summary>
        /// Возвращает все слова
        /// </summary>
        public static IEnumerable<Verb> GetAll()
        {
            if (!Initialized)
                Init();
            foreach (var raw in items)
                yield return new Verb(raw, raw.Word);
        }
    }

    public class CustomWordsNeededEventArgs : EventArgs
    {
        public List<string> CustomWords { get; set; }
    }

    internal struct VerbRaw
    {
        public string Word;
        public int SchemaIndex;
        public VerbAspect Aspect;

        public override string ToString()
        {
            return Word;
        }
    }

    internal struct Pair : IComparable<Pair>
    {
        public string Item1;
        public string Item2;

        public int CompareTo(Pair other)
        {
            return Item1.CompareTo(other.Item1);
        }
    }

    /// <summary>
    /// Глагол и его словоформы
    /// </summary>
    public class Verb
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
        /// Вид глагола
        /// </summary>
        public VerbAspect Aspect { get; internal set; }

        /// <summary>
        /// Транзитивность глагола
        /// </summary>
        public VerbTransition Transition
        {
            get
            {
                return this[Voice.Passive, Person.First, Number.Singular] == null
                           ? VerbTransition.Intransitive
                           : VerbTransition.Transitive;
            }
        }

        internal Verb(VerbRaw raw, string word)
        {
            this.Word = word;
            if (raw.Word != null)
            {
                SchemaIndex = raw.SchemaIndex;
                Aspect = raw.Aspect;
            }
        }

        public Verb()
        {
        }

        /// <summary>
        /// Словоформы глагола по залогу, лицу и числу
        /// </summary>
        public string this[Voice voice, Person person, Number number]
        {
            get
            {
                var i = (int)voice * 78;
                i += (int)number * 3;
                i += (int)person;
                return Verbs.schemas[SchemaIndex].GetForm(Word, i);
            }
        }

        /// <summary>
        /// Деепричастие (спряжение по времени)
        /// </summary>
        public string Gerund(Tense tense)
        {
            var i = tense == Tense.Present ? 6 : 11;
            return Verbs.schemas[SchemaIndex].GetForm(Word, i);
        }

        /// <summary>
        /// Указательная форма глагола (спряжение по залогу и числу)
        /// </summary>
        public string Imperative(Voice voice, Number number)
        {
            var i = (int)voice * 78;
            i += 12  + (int)number;
            return Verbs.schemas[SchemaIndex].GetForm(Word, i);
        }

        /// <summary>
        /// Прошедшее время глагола (спряжение по залогу и роду)
        /// </summary>
        public string Past(Voice voice, Gender gender)
        {
            var i = (int) voice * 78;
            i += 7 + (int) (gender.Gen());
            return Verbs.schemas[SchemaIndex].GetForm(Word, i);
        }

        /// <summary>
        /// Сослагательное/условное наклонение
        /// </summary>
        public string Subjunctive(Voice voice, Gender gender)
        {
            return "бы".ToUpper(Word) + " " + Past(voice, gender);
        }

        /// <summary>
        /// Будущее время глагола - со вспомогательным словом (спряжение по залогу, лицу и числу)
        /// </summary>
        public string Future(Voice voice, Person person, Number number)
        {
            switch(Aspect)
            {
                case VerbAspect.Perfect: return this[voice, person, number];
                case VerbAspect.Imperfect:
                case VerbAspect.PerfectImperfect:
                    if (Transition == VerbTransition.Transitive || voice == Voice.Active)
                        return Verbs.ToBe[Voice.Active, person, number].ToUpper(Word) + " " + Infinitive(voice).ToLower();
                    else 
                        return null;
            }
            return null;
        }

        /// <summary>
        /// Будущее время глагола - в виде глагла совершенного вида (спряжение по залогу, лицу и числу)
        /// </summary>
        public string FuturePerfect(Voice voice, Person person, Number number)
        {
            switch (Aspect)
            {
                case VerbAspect.Perfect: return this[voice, person, number];
                case VerbAspect.Imperfect:
                case VerbAspect.PerfectImperfect:
                    {
                        var perf = Perfect(voice);
                        if (perf != null)
                            return perf[voice, person, number];
                        else
                            return null;
                    }
            }
            return null;
        }

        /// <summary>
        /// Неопределенная форма (по залогу)
        /// </summary>
        public string Infinitive(Voice voice)
        {
            switch(voice)
            {
                case Voice.Active: return Word;
                case Voice.Passive: 
                        return Transition == VerbTransition.Transitive ? Word + "ся" : null;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Причастие (спряжение по залогу, падежу, роду и времени)
        /// </summary>
        public string Participle(Voice voice, Case @case, Gender gender, Tense tense)
        {
            var i = @case.IndexWithAnimate(gender);
            i += (int)voice * 78;
            i += 14;
            i += 8 * (int)gender.Gen();
            i += 32*(int) tense;

            return Verbs.schemas[SchemaIndex].GetForm(Word, i);
        }

        /// <summary>
        /// Совершенная форма глагола
        /// </summary>
        public Verb Perfect(Voice voice)
        {
            foreach (var perf in Perfects(voice))
                return perf;

            return null;
        }

        /// <summary>
        /// Совершенные формы глагола
        /// </summary>
        public IEnumerable<Verb> Perfects(Voice voice)
        {
            if (Aspect == VerbAspect.Perfect)
                yield return this;
            else
            {
                var word = Infinitive(voice);
                if(word != null)
                foreach (var res in Verbs.FindPerfects(word))
                    yield return res;
            }
        }
    }
}
