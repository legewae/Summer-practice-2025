using System;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.CodeDom;

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

        CSharpCodeProvider compiler = new CSharpCodeProvider();
        CompilerParameters parameters = new CompilerParameters
        {
            GenerateInMemory = true,
            GenerateExecutable = false
        };

        CompilerResults results = compiler.CompileAssemblyFromSource(parameters, classCode);

        return Activator.CreateInstance(results.CompiledAssembly.GetType("Calculator"));
    }
}