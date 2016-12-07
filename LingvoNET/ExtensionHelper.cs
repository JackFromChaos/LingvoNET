using System;

namespace LingvoNET
{
    public static class ExtensionHelper
    {
        /// <summary>
        /// Одушевленность
        /// </summary>
        public static Animacy Animacy(this Gender gender)
        {
            switch(gender)
            {
                case Gender.FA:
                case Gender.MA:
                case Gender.MAFA:
                case Gender.NA:
                case Gender.PA:
                    return LingvoNET.Animacy.Animate;
                default:
                    return LingvoNET.Animacy.Inanimate;
            }
        }

        /// <summary>
        /// Род без учета одушевленности (возвращает F, M, N или P)
        /// </summary>
        public static Gender Gen(this Gender gender)
        {
            switch (gender)
            {
                case Gender.F:
                case Gender.FA:
                case Gender.MAFA:
                    return Gender.F;
                case Gender.M:
                case Gender.MA:
                    return Gender.M;
                case Gender.N:
                case Gender.NA:
                    return Gender.N;
                case Gender.P:
                case Gender.PA:
                    return Gender.P;
                default:
                    return Gender.Undefined;
            }
        }

        /// <summary>
        /// Число без учета рода и одушеленности
        /// </summary>
        public static Number Number(this Gender gender)
        {
            switch (gender)
            {
                case Gender.P:
                case Gender.PA:
                    return LingvoNET.Number.Plural;
                default:
                    return LingvoNET.Number.Singular;
            }
        }

        public static int IndexWithAnimate(this Case @case, Gender gender)
        {
            var i = 0;
            switch (@case)
            {
                case Case.Nominative: i = 0; break;
                case Case.Genitive: i = 1; break;
                case Case.Dative: i = 2; break;
                case Case.Accusative: i = gender.Animacy() == LingvoNET.Animacy.Animate ? 4 : 3; break;
                case Case.Instrumental: i = 5; break;
                case Case.Locative: i = 6; break;
                case Case.Short: i = 7; break;
            }

            return i;
        }

        public static string ToUpper(this string s, string sample)
        {
            if (Char.IsUpper(sample[0]))
                return Char.ToUpper(s[0]) + s.Substring(1).ToLower();
            else
                return s.ToLower();
        }

        public static string ToUpperFirst(this string s)
        {
            if(!string.IsNullOrEmpty(s))
                return Char.ToUpper(s[0]) + s.Substring(1);

            return s;
        }
    }
}