using Markdig.Renderers;
using ColorCode;
using Markdig.Xmd.CSCode;
using Markdig.Xmd.Alert;

namespace Markdig.Xmd
{
    public class XMDLanguageExtension : IMarkdownExtension
    {
        private readonly IStyleSheet _customCss;

        public XMDLanguageExtension(IStyleSheet customCss = null)
        {
            _customCss = customCss;
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {

            if (!pipeline.InlineParsers.Contains<AlertBlockParser>())
            {
                pipeline.InlineParsers.Add(new AlertBlockParser(pipeline));
            }

            if (!pipeline.InlineParsers.Contains<CSCodeInlineParser>())
            {
                pipeline.InlineParsers.Add(new CSCodeInlineParser());
            }

            if (!pipeline.InlineParsers.Contains<CSCodeBlockParser>())
            {
                pipeline.InlineParsers.Add(new CSCodeBlockParser());
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {

            if (!renderer.ObjectRenderers.Contains<AlertBlockRenderer>())
            {
                renderer.ObjectRenderers.Add(new AlertBlockRenderer(pipeline));
            }

            if (!renderer.ObjectRenderers.Contains<CSCodeInlineRenderer>())
            {
                renderer.ObjectRenderers.Add(new CSCodeInlineRenderer());
            }

            if (!renderer.ObjectRenderers.Contains<CSCodeBlockRenderer>())
            {
                renderer.ObjectRenderers.Add(new CSCodeBlockRenderer());
            }
        }

    }
}
