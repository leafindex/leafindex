20 Jan 2009 : 17:10

Make sure you have a c:\CSharp directory, but no LeafIndex directory below it
Use Git to Clone a depository : git@github.com:leafindex/leafindex.git cloned to c:\csharp\leafindex

From here the challenge is 

	* open the .sln file
	* build the ScLib project (uses xunit - may need to install)
	* build the website (may need to reference sclib)
	* set up http://localhost/leafindex on IIS to point to c:\csharp\leafindex\website
	* default document is ACOAssaults.htm
	* point your browser at http://localhost/leafindex


	