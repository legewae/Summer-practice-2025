using System;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.CodeDom;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

public class Calc
{
    public object CreateCalculator()
    {
        string classCode = @"public class Calculator
        {
        public int Add(int a, int b) => a + b;
        public int Minus(int a, int b) => a - b;
        public int Mul(int a, int b) => a * b;
        public int Div(int a, int b) => a / b;
        }";

        var compilation = CSharpCompilation.Create("assemblator3000")
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(CSharpSyntaxTree.ParseText(classCode));

        var memoryStream = new MemoryStream();
        var emit = compilation.Emit(memoryStream);
        var assembly = Assembly.Load(memoryStream.ToArray());

        return assembly.CreateInstance("Calculator")!;
    }
}