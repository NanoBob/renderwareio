using RenderWareBuilders;
using System;
using System.Linq;
using System.Text;

namespace RenderWareIo.Mta
{
    public class MtaDebugView
    {
        private readonly RenderWareBuilder renderWareBuilder;

        public MtaDebugView(RenderWareBuilder renderWareBuilder)
        {
            this.renderWareBuilder = renderWareBuilder;
        }

        public string Vertices
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendLine("function drawWireframe()");
                {
                    sb.AppendLine("\tlocal debugOffset = debugOffset or {0,0,0}");
                    sb.AppendLine("\tlocal debugTextColor = debugTextColor or tocolor(255,255,255)");
                    sb.AppendLine("\tlocal debugTextSize = debugTextSize or 2");
                    sb.AppendLine("\tlocal debugTextFont = debugTextFont or \"sans\"");
                    sb.AppendLine("\tlocal vertices = {");
                    if (renderWareBuilder.vertices.Any())
                    {
                        foreach (var vertex in renderWareBuilder.vertices)
                        {
                            sb.AppendLine($"\t\t[{vertex.Index + 1}] = {{{vertex.Position.X},{vertex.Position.Y},{vertex.Position.Z}}},");
                        }
                    }
                    else
                    {
                        foreach (var vertex in renderWareBuilder.prelitVertices)
                        {
                            sb.AppendLine($"\t\t[{vertex.Index + 1}] = {{{vertex.Position.X},{vertex.Position.Y},{vertex.Position.Z}}},");
                        }
                    }
                    sb.AppendLine("\t}");

                    sb.AppendLine("\tfor i,v in ipairs(vertices)do");
                    sb.AppendLine("\t\tlocal sx,sy = getScreenFromWorldPosition(v[1] + debugOffset[1], v[2] + debugOffset[2], v[3] + debugOffset[3], 128, false)");
                    sb.AppendLine("\t\tif sx and sy then");
                    sb.AppendLine("\t\t\tdxDrawText(i - 1, sx, sy, sx, sy, debugTextColor, debugTextSize, debugTextFont, \"center\", \"center\")");
                    sb.AppendLine("\t\tend");
                    sb.AppendLine("\tend");
                }
                sb.AppendLine("end");
                sb.AppendLine("addEventHandler(\"onClientRender\", root, drawWireframe)");
                return sb.ToString();
            }
        }

        public string Wireframe
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendLine("function drawWireframe()");
                {
                    sb.AppendLine("\tlocal debugOffset = debugOffset or {0,0,0}");
                    sb.AppendLine("\tlocal lineWidth = lineWidth or 2");
                    sb.AppendLine("\tlocal lineColor = lineColor or tocolor(255,255,255,255)");
                    sb.AppendLine("\tlocal vertices = {");
                    if (renderWareBuilder.vertices.Any())
                    {
                        foreach (var vertex in renderWareBuilder.vertices)
                        {
                            sb.AppendLine($"\t\t[{vertex.Index}] = {{{vertex.Position.X},{vertex.Position.Y},{vertex.Position.Z}}},");
                        }
                    }
                    else
                    {
                        foreach (var vertex in renderWareBuilder.prelitVertices)
                        {
                            sb.AppendLine($"\t\t[{vertex.Index}] = {{{vertex.Position.X},{vertex.Position.Y},{vertex.Position.Z}}},");
                        }

                    }
                    sb.AppendLine("\t}");

                    sb.AppendLine("\tlocal triangles = {");
                    foreach (var triangle in renderWareBuilder.triangles)
                    {
                        sb.AppendLine($"\t\t[{triangle.Index}] = {{{triangle.Vertex1.Index},{triangle.Vertex2.Index},{triangle.Vertex3.Index}}},");
                    }
                    sb.AppendLine("\t}\n");
                    sb.AppendLine("\tfor i,v in pairs(triangles)do");
                    sb.AppendLine("\t\t v1 = vertices[v[1]]");
                    sb.AppendLine("\t\t v2 = vertices[v[2]]");
                    sb.AppendLine("\t\t v3 = vertices[v[3]]");
                    sb.AppendLine("\t\tdxDrawLine3D(v1[1] + debugOffset[1],v1[2] + debugOffset[2],v1[3] + debugOffset[3], v2[1] + debugOffset[1],v2[2] + debugOffset[2],v2[3] + debugOffset[3], lineColor, lineWidth)");
                    sb.AppendLine("\t\tdxDrawLine3D(v1[1] + debugOffset[1],v1[2] + debugOffset[2],v1[3] + debugOffset[3], v3[1] + debugOffset[1],v3[2] + debugOffset[2],v3[3] + debugOffset[3], lineColor, lineWidth)");
                    sb.AppendLine("\t\tdxDrawLine3D(v2[1] + debugOffset[1],v2[2] + debugOffset[2],v2[3] + debugOffset[3], v3[1] + debugOffset[1],v3[2] + debugOffset[2],v3[3] + debugOffset[3], lineColor, lineWidth)");
                    sb.AppendLine("\tend");


                }
                sb.AppendLine("end");
                sb.AppendLine("addEventHandler(\"onClientRender\", root, drawWireframe)");
                return sb.ToString();
            }
        }
    }
}
