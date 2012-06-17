﻿using System;
using System.Collections.Generic;

using System.Text;
using Antlr.Runtime.Tree;
using SGL.Elements;
using SGL.Antlr;

namespace SGL.Library.Functions
{
    /// <summary>
    /// A method defined by "method [name]() { ... } in the actuall SGL code."
    /// </summary>
    class UserFunction : Function
    {
        private List<String> parameterNames;
        private CommonTreeNodeStream functionBody;
        private String name;
        private int definedLine;

        // method xyt(param1, param2) {
        public UserFunction(String name, List<String> parameterNames, CommonTreeNodeStream functionBody, int definedLine)
        {
            this.parameterNames = parameterNames;
            this.functionBody = functionBody;
            this.name = name;
            this.definedLine = definedLine;
        }

        public override Value Invoke(List<Value> parameters)
        {
            if (parameters.Count == parameterNames.Count)
            {
                // create new scope for the method call
                Scope methodScope = new Scope();
                for (int i = 0; i < parameterNames.Count; i++)
                {
                    methodScope.Assign(parameterNames[i], parameters[i]);
                }

                // Create a tree walker to evaluate this method's code block  
                SGLTreeWalker walker = new SGLTreeWalker(functionBody, methodScope);

                // Ok, executing the function then
                Value returnValue = walker.main().Evaluate();

                // we shouldn't check the return type
                /*if (!returnValue.GetVarType().Equals(this.returnType))
                {
                    throw new Exception("The method doesn't return the expected return type (" + returnValue.ToString()  + " is not from type " + this.returnType + ")");
                }*/
                return returnValue;
            }
            // TODO: Exception
            throw new Exception();
        }


    }
}