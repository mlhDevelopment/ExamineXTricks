# Defacet

Code example of disabling the default Is Facetable setting created during the Azure index create process.

## The Problem

ExamineX creates indexes with many Azure Cognitive Search (AGC) settings set as default. Facetable is one of those settings, defaulting to `true`.

[Azure imposes a size limit on facetable fields](https://stackoverflow.com/a/37150145/295686), 32KB. If a field is sent over to be indexed 
and it is over 32KB the index requst (to Azure) fails. ExamineX will print a message at the debug level and continue to process other fields:


    dbug: ExamineX.AzureSearch.AzureSearchIndex[0]
          (ProcessIndexBatch) Batch had partial failure, retrying 1 failed document.
          Document failed: 1234, Error: Field 'longContent' contains a term that is too large to process. The max length for UTF-8 encoded terms is 32766 bytes. The most likely cause of this error is that filtering, sorting, and/or faceting are enabled on this field, which causes the entire field value to be indexed as a single term. Please avoid the use of these options for large fields.

You can verify the field is missing by opening the index's Search Explorer in AGC and searching for content you have verified should be there
(but no results come up). If you know of a particular page that doesn't come back in ExamineX's search results you can search for the page id 
or page guid.

In AGC you can examine the Fields tab and verify the field is there but has Facetable selected. It cannot be changed after the index is created.

## The Solution

Create an [Umbraco Component](https://our.umbraco.com/documentation/Implementation/Composing/#componentcomposer) that listens for the 
`CreatingOrUpdatingIndex` event. You can set the IsFacetable property there.

See `NotFacetableExamineXComponent.cs` for an example of that setup.
