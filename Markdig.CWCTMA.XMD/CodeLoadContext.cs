﻿using System;
using System.Reflection;
using System.Runtime.Loader;

namespace Markdig.CWCTMA.XMD
{
    internal sealed class CodeLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver resolver;

        public CodeLoadContext(string name) : base(name: name, isCollectible: true)
        {
        }

        public CodeLoadContext(string name, string path) : base(name: name, isCollectible: true)
        {
            resolver = new AssemblyDependencyResolver(path);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = resolver?.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            return IntPtr.Zero;
        }
    }
}