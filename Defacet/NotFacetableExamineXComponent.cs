using Examine;
using ExamineX.AzureSearch;
using System.Linq;
using Umbraco.Cms.Core.Composing;

namespace HCF.ExamineXTricks.Defacet
{
    public class NotFacetableExamineXComposer : ComponentComposer<NotFacetableExamineXComponent>
    { }

    public class NotFacetableExamineXComponent : IComponent
    {
        private readonly IExamineManager _examineManager;
        private readonly string[] _unfacetableFields = new [] { "longContent", "massiveField" };

        public NotFacetableExamineXComponent(
            IExamineManager examineManager)
        {
            _examineManager = examineManager;
        }

        private void AzureIndex_CreatingOrUpdatingIndex(object? sender, CreatingOrUpdatingIndexEventArgs e)
        {
            foreach (var searchField in e.AzureSearchIndexDefinition.Fields)
            {
                if (_unfacetableFields.Contains(searchField.Name))
                {
                    searchField.IsFacetable = false;
                }
            }
        }

        public void Initialize()
        {
            if (_examineManager.TryGetIndex("ExternalIndex", out var index) && index is AzureSearchIndex azureIndex)
            {
                azureIndex.CreatingOrUpdatingIndex += AzureIndex_CreatingOrUpdatingIndex;
            }
        }

        public void Terminate()
        {
        }
    }
}