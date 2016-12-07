using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using LingvoNET;

namespace Tester
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            foreach (var i in Enum.GetValues(typeof(Gender)))
                cbGender.Items.Add(i);

            cbGender.SelectedItem = Gender.Undefined;

            foreach (var i in Enum.GetValues(typeof(Animacy)))
                cbAnimacy.Items.Add(i);

            cbAnimacy.SelectedItem = Animacy.Undefined;

            foreach (var i in Enum.GetValues(typeof(Comparability)))
                cbComparability.Items.Add(i);

            cbComparability.SelectedItem = Comparability.Undefined;

            foreach (var i in Enum.GetValues(typeof(VerbAspect)))
                cbAspect.Items.Add(i);

            cbAspect.SelectedItem = VerbAspect.Undefined;

            Nouns.BeforeFindSimilar += Nouns_BeforeFindSimilar;
        }

        HashSet<string> MenNames = new HashSet<string>() { "Петр", "Паша", "Женя", "Костя", "Алеша", "Леша", "Саша", "Славик" };

        void Nouns_BeforeFindSimilar(object sender, BeforeFindSimilarNounEventArgs e)
        {
            if (MenNames.Contains(e.SourceForm))
                e.Gender = Gender.MA;
        }

        private void btFindAll_Click(object sender, EventArgs e)
        {
            tbRes.Clear();
            var sb = new StringBuilder();

            foreach (var v in Verbs.FindAll(tbWord.Text))
                BuildString(sb, v);

            foreach (var n in Nouns.FindAll(tbWord.Text))
                BuildString(sb, n);

            foreach (var a in Adjectives.FindAll(tbWord.Text))
                BuildString(sb, a);

            tbRes.Text = sb.ToString();
        }

        private void btFindSimilar_Click(object sender, EventArgs e)
        {
            tbRes.Clear();
            var sb = new StringBuilder();

            var v = Verbs.FindSimilar(tbWord.Text, (VerbAspect)cbAspect.SelectedItem);
            if (v != null)
                BuildString(sb, v);

            var n = Nouns.FindSimilar(tbWord.Text, (Gender)cbGender.SelectedItem, (Animacy)cbAnimacy.SelectedItem);
            if (n != null)
                BuildString(sb, n);

            var a = Adjectives.FindSimilar(tbWord.Text, (Comparability)cbComparability.SelectedItem);
            if (a != null)
                BuildString(sb, a);

            tbRes.Text = sb.ToString();
        }

        private void tbWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btFindSimilar.PerformClick();
        }

        private static void BuildString(StringBuilder sb, Verb v)
        {
            sb.AppendFormat("---------------------------------Verb {0} {2} {1}\r\n", v.Aspect, v.Inexact ? "(inexactly)" : "", v.Transition);
            BuildString(sb, v, Voice.Active);
            BuildString(sb, v, Voice.Passive);
        }

        private static void BuildString(StringBuilder sb, Noun n)
        {
            sb.AppendFormat(
                @"---------------------------------Noun {0} {13}

Nominative:   {1}   {2}
Genitive:     {3}   {4}
Dative:       {5}   {6}
Accusative:   {7}   {8}
Instrumental: {9}   {10}
Locative:     {11}   {12}

",
                n.Gender,
                PadLeft(n[Case.Nominative]), PadLeft(n[Case.Nominative, Number.Plural]),
                PadLeft(n[Case.Genitive]), PadLeft(n[Case.Genitive, Number.Plural]),
                PadLeft(n[Case.Dative]), PadLeft(n[Case.Dative, Number.Plural]),
                PadLeft(n[Case.Accusative]), PadLeft(n[Case.Accusative, Number.Plural]),
                PadLeft(n[Case.Instrumental]), PadLeft(n[Case.Instrumental, Number.Plural]),
                PadLeft(n[Case.Locative]), PadLeft(n[Case.Locative, Number.Plural]),
                n.Inexact ? "(inexactly)" : "");
        }

        private static void BuildString(StringBuilder sb, Adjective a)
        {
            sb.AppendFormat("---------------------------------Adj {0} {1}\r\n\r\n", a.Comparability, a.Inexact ? "(inexactly)" : "");
            sb.AppendFormat("Nominative:            {0}   {1}   {2}   {3}\r\n", PadLeft(a[Case.Nominative, Gender.M]), PadLeft(a[Case.Nominative, Gender.F]), PadLeft(a[Case.Nominative, Gender.N]), PadLeft(a[Case.Nominative, Gender.P]));
            sb.AppendFormat("Genitive:              {0}   {1}   {2}   {3}\r\n", PadLeft(a[Case.Genitive, Gender.M]), PadLeft(a[Case.Genitive, Gender.F]), PadLeft(a[Case.Genitive, Gender.N]), PadLeft(a[Case.Genitive, Gender.P]));
            sb.AppendFormat("Dative:                {0}   {1}   {2}   {3}\r\n", PadLeft(a[Case.Dative, Gender.M]), PadLeft(a[Case.Dative, Gender.F]), PadLeft(a[Case.Dative, Gender.N]), PadLeft(a[Case.Dative, Gender.P]));
            sb.AppendFormat("Accusative Inanimate:  {0}   {1}   {2}   {3}\r\n", PadLeft(a[Case.Accusative, Gender.M]), PadLeft(a[Case.Accusative, Gender.F]), PadLeft(a[Case.Accusative, Gender.N]), PadLeft(a[Case.Accusative, Gender.P]));
            sb.AppendFormat("Accusative Animate:    {0}   {1}   {2}   {3}\r\n", PadLeft(a[Case.Accusative, Gender.MA]), PadLeft(a[Case.Accusative, Gender.FA]), PadLeft(a[Case.Accusative, Gender.NA]), PadLeft(a[Case.Accusative, Gender.PA]));
            sb.AppendFormat("Instrumental:          {0}   {1}   {2}   {3}\r\n", PadLeft(a[Case.Instrumental, Gender.M]), PadLeft(a[Case.Instrumental, Gender.F]), PadLeft(a[Case.Instrumental, Gender.N]), PadLeft(a[Case.Instrumental, Gender.P]));
            sb.AppendFormat("Locative:              {0}   {1}   {2}   {3}\r\n", PadLeft(a[Case.Locative, Gender.M]), PadLeft(a[Case.Locative, Gender.F]), PadLeft(a[Case.Locative, Gender.N]), PadLeft(a[Case.Locative, Gender.P]));
            sb.AppendFormat("Short:                 {0}   {1}   {2}   {3}\r\n", PadLeft(a[Case.Short, Gender.M]), PadLeft(a[Case.Short, Gender.F]), PadLeft(a[Case.Short, Gender.N]), PadLeft(a[Case.Short, Gender.P]));
            sb.AppendLine();
            sb.AppendFormat("Comparative:           {0}   {1}   {2}   {3}\r\n", PadLeft(a[Comparison.Comparative1]), PadLeft(a[Comparison.Comparative2]), PadLeft(a[Comparison.Comparative3]), PadLeft(a[Comparison.Comparative4]));
        }

        private static void BuildString(StringBuilder sb, Verb v, Voice voice)
        {
            sb.AppendFormat("----{0}:   {1}\r\n\r\n", voice, PadLeft(v.Infinitive(voice)));

            sb.AppendFormat("Present\r\n");
            sb.AppendFormat("First:    {0}  {1}\r\n", PadLeft(v[voice, Person.First, Number.Singular]), PadLeft(v[voice, Person.First, Number.Plural]));
            sb.AppendFormat("Second:   {0}  {1}\r\n", PadLeft(v[voice, Person.Second, Number.Singular]), PadLeft(v[voice, Person.Second, Number.Plural]));
            sb.AppendFormat("Third:    {0}  {1}\r\n\r\n", PadLeft(v[voice, Person.Third, Number.Singular]), PadLeft(v[voice, Person.Third, Number.Plural]));

            if (v.Aspect != VerbAspect.Perfect)
            {
                sb.AppendFormat("Future\r\n");
                sb.AppendFormat("First:    {0}  {1}    {2}  {3}\r\n", PadLeft(v.Future(voice, Person.First, Number.Singular), 20), PadLeft(v.Future(voice, Person.First, Number.Plural), 20), PadLeft(v.FuturePerfect(voice, Person.First, Number.Singular)), PadLeft(v.FuturePerfect(voice, Person.First, Number.Plural)));
                sb.AppendFormat("Second:   {0}  {1}    {2}  {3}\r\n", PadLeft(v.Future(voice, Person.Second, Number.Singular), 20), PadLeft(v.Future(voice, Person.Second, Number.Plural), 20), PadLeft(v.FuturePerfect(voice, Person.Second, Number.Singular)), PadLeft(v.FuturePerfect(voice, Person.Second, Number.Plural)));
                sb.AppendFormat("Third:    {0}  {1}    {2}  {3}\r\n\r\n", PadLeft(v.Future(voice, Person.Third, Number.Singular), 20), PadLeft(v.Future(voice, Person.Third, Number.Plural), 20), PadLeft(v.FuturePerfect(voice, Person.Third, Number.Singular)), PadLeft(v.FuturePerfect(voice, Person.Third, Number.Plural)));
            }

            sb.AppendFormat("Past:     {0}  {1}  {2}  {3}\r\n\r\n", PadLeft(v.Past(voice, Gender.M)), PadLeft(v.Past(voice, Gender.F)), PadLeft(v.Past(voice, Gender.N)), PadLeft(v.Past(voice, Gender.P)));

            sb.AppendFormat("Perfect:     ");
            foreach(var perf in v.Perfects(voice))
                sb.Append(PadLeft(perf.Infinitive(voice)));
            sb.AppendLine();


            if(voice == Voice.Active)
                sb.AppendFormat("Gerund:      {0}   {1}\r\n", PadLeft(v.Gerund(Tense.Present)), PadLeft(v.Gerund(Tense.Past)));
            sb.AppendFormat("Imperative:  {0}   {1}\r\n", PadLeft(v.Imperative(voice, Number.Singular)), PadLeft(v.Imperative(voice, Number.Plural)));
            

            sb.AppendFormat("\r\n");
            sb.AppendFormat("Present participle\r\n");

            sb.AppendFormat("Nominative:            {0}   {1}   {2}   {3}\r\n", PadLeft(v.Participle(voice, Case.Nominative, Gender.M, Tense.Present)), PadLeft(v.Participle(voice, Case.Nominative, Gender.F, Tense.Present)), PadLeft(v.Participle(voice, Case.Nominative, Gender.N, Tense.Present)), PadLeft(v.Participle(voice, Case.Nominative, Gender.P, Tense.Present)));
            sb.AppendFormat("Genitive:              {0}   {1}   {2}   {3}\r\n", PadLeft(v.Participle(voice, Case.Genitive, Gender.M, Tense.Present)), PadLeft(v.Participle(voice, Case.Genitive, Gender.F, Tense.Present)), PadLeft(v.Participle(voice, Case.Genitive, Gender.N, Tense.Present)), PadLeft(v.Participle(voice, Case.Genitive, Gender.P, Tense.Present)));
            sb.AppendFormat("Dative:                {0}   {1}   {2}   {3}\r\n", PadLeft(v.Participle(voice, Case.Dative, Gender.M, Tense.Present)), PadLeft(v.Participle(voice, Case.Dative, Gender.F, Tense.Present)), PadLeft(v.Participle(voice, Case.Dative, Gender.N, Tense.Present)), PadLeft(v.Participle(voice, Case.Dative, Gender.P, Tense.Present)));
            sb.AppendFormat("Accusative Inanimate:  {0}   {1}   {2}   {3}\r\n", PadLeft(v.Participle(voice, Case.Accusative, Gender.M, Tense.Present)), PadLeft(v.Participle(voice, Case.Accusative, Gender.F, Tense.Present)), PadLeft(v.Participle(voice, Case.Accusative, Gender.N, Tense.Present)), PadLeft(v.Participle(voice, Case.Accusative, Gender.P, Tense.Present)));
            sb.AppendFormat("Accusative Animate:    {0}   {1}   {2}   {3}\r\n", PadLeft(v.Participle(voice, Case.Accusative, Gender.MA, Tense.Present)), PadLeft(v.Participle(voice, Case.Accusative, Gender.FA, Tense.Present)), PadLeft(v.Participle(voice, Case.Accusative, Gender.NA, Tense.Present)), PadLeft(v.Participle(voice, Case.Accusative, Gender.PA, Tense.Present)));
            sb.AppendFormat("Instrumental:          {0}   {1}   {2}   {3}\r\n", PadLeft(v.Participle(voice, Case.Instrumental, Gender.M, Tense.Present)), PadLeft(v.Participle(voice, Case.Instrumental, Gender.F, Tense.Present)), PadLeft(v.Participle(voice, Case.Instrumental, Gender.N, Tense.Present)), PadLeft(v.Participle(voice, Case.Instrumental, Gender.P, Tense.Present)));
            sb.AppendFormat("Locative:              {0}   {1}   {2}   {3}\r\n", PadLeft(v.Participle(voice, Case.Locative, Gender.M, Tense.Present)), PadLeft(v.Participle(voice, Case.Locative, Gender.F, Tense.Present)), PadLeft(v.Participle(voice, Case.Locative, Gender.N, Tense.Present)), PadLeft(v.Participle(voice, Case.Locative, Gender.P, Tense.Present)));
            sb.AppendFormat("Short:                 {0}   {1}   {2}   {3}\r\n", PadLeft(v.Participle(voice, Case.Short, Gender.M, Tense.Present)), PadLeft(v.Participle(voice, Case.Short, Gender.F, Tense.Present)), PadLeft(v.Participle(voice, Case.Short, Gender.N, Tense.Present)), PadLeft(v.Participle(voice, Case.Short, Gender.P, Tense.Present)));
            sb.AppendLine();

            sb.AppendFormat("\r\n");
            sb.AppendFormat("Past participle\r\n");

            sb.AppendFormat("Nominative:            {0}   {1}   {2}   {3}\r\n", PadLeft(v.Participle(voice, Case.Nominative, Gender.M, Tense.Past)), PadLeft(v.Participle(voice, Case.Nominative, Gender.F, Tense.Past)), PadLeft(v.Participle(voice, Case.Nominative, Gender.N, Tense.Past)), PadLeft(v.Participle(voice, Case.Nominative, Gender.P, Tense.Past)));
            sb.AppendFormat("Genitive:              {0}   {1}   {2}   {3}\r\n", PadLeft(v.Participle(voice, Case.Genitive, Gender.M, Tense.Past)), PadLeft(v.Participle(voice, Case.Genitive, Gender.F, Tense.Past)), PadLeft(v.Participle(voice, Case.Genitive, Gender.N, Tense.Past)), PadLeft(v.Participle(voice, Case.Genitive, Gender.P, Tense.Past)));
            sb.AppendFormat("Dative:                {0}   {1}   {2}   {3}\r\n", PadLeft(v.Participle(voice, Case.Dative, Gender.M, Tense.Past)), PadLeft(v.Participle(voice, Case.Dative, Gender.F, Tense.Past)), PadLeft(v.Participle(voice, Case.Dative, Gender.N, Tense.Past)), PadLeft(v.Participle(voice, Case.Dative, Gender.P, Tense.Past)));
            sb.AppendFormat("Accusative Inanimate:  {0}   {1}   {2}   {3}\r\n", PadLeft(v.Participle(voice, Case.Accusative, Gender.M, Tense.Past)), PadLeft(v.Participle(voice, Case.Accusative, Gender.F, Tense.Past)), PadLeft(v.Participle(voice, Case.Accusative, Gender.N, Tense.Past)), PadLeft(v.Participle(voice, Case.Accusative, Gender.P, Tense.Past)));
            sb.AppendFormat("Accusative Animate:    {0}   {1}   {2}   {3}\r\n", PadLeft(v.Participle(voice, Case.Accusative, Gender.MA, Tense.Past)), PadLeft(v.Participle(voice, Case.Accusative, Gender.FA, Tense.Past)), PadLeft(v.Participle(voice, Case.Accusative, Gender.NA, Tense.Past)), PadLeft(v.Participle(voice, Case.Accusative, Gender.PA, Tense.Past)));
            sb.AppendFormat("Instrumental:          {0}   {1}   {2}   {3}\r\n", PadLeft(v.Participle(voice, Case.Instrumental, Gender.M, Tense.Past)), PadLeft(v.Participle(voice, Case.Instrumental, Gender.F, Tense.Past)), PadLeft(v.Participle(voice, Case.Instrumental, Gender.N, Tense.Past)), PadLeft(v.Participle(voice, Case.Instrumental, Gender.P, Tense.Past)));
            sb.AppendFormat("Locative:              {0}   {1}   {2}   {3}\r\n", PadLeft(v.Participle(voice, Case.Locative, Gender.M, Tense.Past)), PadLeft(v.Participle(voice, Case.Locative, Gender.F, Tense.Past)), PadLeft(v.Participle(voice, Case.Locative, Gender.N, Tense.Past)), PadLeft(v.Participle(voice, Case.Locative, Gender.P, Tense.Past)));
            sb.AppendFormat("Short:                 {0}   {1}   {2}   {3}\r\n", PadLeft(v.Participle(voice, Case.Short, Gender.M, Tense.Past)), PadLeft(v.Participle(voice, Case.Short, Gender.F, Tense.Past)), PadLeft(v.Participle(voice, Case.Short, Gender.N, Tense.Past)), PadLeft(v.Participle(voice, Case.Short, Gender.P, Tense.Past)));
            sb.AppendLine();
        }

        static string PadLeft(string s, int indent = 15)
        {
            if (s == null)
                s = "";
            return s.PadRight(indent);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            btFindAll.PerformClick();
        }
    }
}
