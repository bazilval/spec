using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spec.Model
{
    public class Project
    {
        public List<Element> Elements { get; set; }
        public List<AssemblyElement> Assemblies { get; set; }
        public HashSet<string> ElementNames { get; set; }
        public HashSet<string> AssemblyNames { get; set; }
        public Project()
        {
            Elements = new List<Element>();
            Assemblies = new List<AssemblyElement>();
            ElementNames = new HashSet<string>();
            AssemblyNames = new HashSet<string>();
        }

        public void AddElement(Element element)
        {
            if (ElementNames.Add(element.Name))
            {
                Elements.Add(element);
            }
            else
            {
                throw new ArgumentException($"Элемент \"{element.Name}\" уже существует!");
            }
        }
        public void RemoveElement(Element element)
        {
            if (ElementNames.Remove(element.Name))
            {
                Elements.Remove(element);
            }
        }

        public void DuplicateElement(Element element, string name)
        {
            if (ElementNames.Add(name))
            {
                Element newElement = new Element(name, element.Type, element.Count, element.Details, element.Materials, element.Embeddeds, element.Marks);
                Elements.Add(newElement);
            }
            else
            {
                throw new ArgumentException($"Элемент \"{name}\" уже существует!");
            }
        }

        public void AddAssembly(AssemblyElement assembly)
        {
            if (AssemblyNames.Add(assembly.Name))
            {
                Assemblies.Add(assembly);
            }
            else
            {
                throw new ArgumentException("Сборка с таким именем уже существует!");
            }
        }
        public void RemoveAssembly(AssemblyElement assembly)
        {
            if (AssemblyNames.Remove(assembly.Name))
            {
                Assemblies.Remove(assembly);
            }
        }
    }
}
