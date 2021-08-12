﻿using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Markdig.CWCTMA.XMD
{
    public class CSCodeInlineRenderer : HtmlObjectRenderer<CSCodeInline>
    {

        protected override void Write(HtmlRenderer renderer, CSCodeInline obj)
        {
            renderer.Write($"{CodeHelper.ExecuteInline(obj.SourceCode)}");
        }
    }
}