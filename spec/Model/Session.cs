using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spec.Model
{
    internal class Session
    {
        public List<Element> Elements { get; set; }
        public List<ElementAssembly> Assemblies { get; set; }
        public HashSet<string> ElementNames { get; set; }
        public HashSet<string> AssemblyNames { get; set; }
        public Session()
        {
            Elements = new List<Element>();
            Assemblies = new List<ElementAssembly>();
        }

        public void AddElement(Element element)
        {
            if (ElementNames.Add(element.Name))
            {
                Elements.Add(element);
            }
            else
            {
                throw new ArgumentException("Элемент с таким именем уже существует!");
            }
        }
        public void RemoveElement(Element element)
        {
            if (ElementNames.Remove(element.Name))
            {
                Elements.Remove(element);
            }
        }

        public void AddAssembly(ElementAssembly assembly)
        {
            if (AssemblyNames.Add(assembly.Name))
            {
                AssemblyNames.Add(assembly);
            }
            else
            {
                throw new ArgumentException("Сборка с таким именем уже существует!");
            }
        }
        public void RemoveAssembly(ElementAssembly assembly)
        {
            if (AssemblyNames.Remove(assembly.Name))
            {
                Assemblies.Remove(assembly);
            }
        }
    }
}
