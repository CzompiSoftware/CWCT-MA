﻿using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Markdig.CWCTMA.XMD
{
    /// <summary>
    /// An inline parser for <see cref="CSCodeInline"/>.
    /// </summary>
    /// <seealso cref="InlineParser" />
    /// <seealso cref="IPostInlineProcessor" />
    public class CSCodeInlineParser : InlineParser
    {
        public string OpeningCharacterString { get; }
        public char[] ClosingCharacters { get; }

        /// <summary>
        /// Gets or sets the default class to use when creating a math inline block.
        /// </summary>
        public string DefaultClass { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CSCodeParser"/> class.
        /// </summary>
        public CSCodeInlineParser()
        {
            OpeningCharacterString = "@cs{#";
            OpeningCharacters = new char[] { OpeningCharacterString[0] };
            ClosingCharacters = "#}".ToArray();
            DefaultClass = "math";
        }


        //public override bool Match(InlineProcessor processor, ref StringSlice slice)
        //{
        //    var match = slice.CurrentChar;
        //    var pc = slice.PeekCharExtra(-1);
        //    if (pc == match)
        //    {
        //        return false;
        //    }
        //    var text = slice.Text;

        //    var start = text.IndexOf(string.Join("", OpeningCharacters));
        //    var end = text.IndexOf(string.Join("", ClosingCharacters))+ ClosingCharacters.Length;

        //    if (start == -1) return false;
        //    if (end == -1) return false;
        //    bool isMatching = false;

        //    /*Regex regex = new Regex("@cs{#.*#}");
        //    Match regexMatch = regex.Match(text);
        //    IEnumerable<Group> matches = regexMatch.Groups.Values;
        //    matches = !matches.Any() ? null : matches;
        //    if(matches != null && !string.IsNullOrEmpty(matches.First().Value))
        //        isMatching = true;
        //    Debug.WriteLine($"--- Matches ---");
        //    foreach (var match in matches)
        //    {
        //        if (string.IsNullOrEmpty(match.Value)) continue;
        //        string cleanCode = match.Value.TrimStart(OpeningCharacters).TrimEnd(ClosingCharacters);

        //        var result = CodeHelper.ExecuteInline(cleanCode);
        //        Debug.WriteLine($"CleanText: {cleanCode}");
        //        Debug.WriteLine($"Code execution result: {result}");
        //        Debug.WriteLine($"---------------");
        //    }*/
        //    string sourceCode = text[start..end];
        //    // Create a new MathInline
        //    var inline = new CSCodeInline()
        //    {
        //        Span = new SourceSpan(processor.GetSourcePosition(slice.Start, out int line, out int column), processor.GetSourcePosition(slice.End)),
        //        Line = line,
        //        Column = column,
        //        Content = new StringSlice(text.Replace(sourceCode, "{{placeholder}}"))
        //    };

        //    // Add the default class if necessary
        //    if (DefaultClass != null)
        //    {
        //        inline.GetAttributes().AddClass(DefaultClass);
        //    }
        //    //processor.Inline = inline;
        //    isMatching = true;
        //    slice = inline.Content;
        //    return isMatching;
        //}

        public override bool Match(InlineProcessor processor, ref StringSlice slice)
        {
            string text = slice.Text;
            if (string.IsNullOrEmpty(text)) return false;
            if(!(text.Contains(OpeningCharacterString) && text.Contains(string.Join("", ClosingCharacters)))){
                return false;
            }

            var start = text.IndexOf(OpeningCharacterString);
            var end = text.IndexOf(string.Join("", ClosingCharacters)) + ClosingCharacters.Length;

            if (start == -1) return false;
            if (end == -1) return false;
            string sourceCode = text[start..end].Replace(OpeningCharacterString, "").Replace(string.Join("", ClosingCharacters), "");
            int length = sourceCode.Length;
            for (int i = 0; i < $"{OpeningCharacterString}{sourceCode}{string.Join("", ClosingCharacters)}".Length; i++)
            {
                slice.NextChar();
            }
            processor.Inline = new CSCodeInline() { SourceCode = sourceCode };
            processor.Inline.Span = new SourceSpan() { Start = processor.GetSourcePosition(slice.Start, out int line, out int column) };
            processor.Inline.Line = line;
            processor.Inline.Span.End = processor.Inline.Span.Start + (start - end - 1);
            return true;
        }
    }
}