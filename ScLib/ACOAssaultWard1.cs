namespace ScLib
{
    public class ACOAssaultWard1
    {
        string _wardName;
				int? _assaultCount;

				public ACOAssaultWard1(string wardName, int? assaultCount)
        {
					_wardName = wardName;
						_assaultCount = assaultCount;
        }

				public string WardName { get { return _wardName; } }
				public int? AssaultCount { get { return _assaultCount; } }
    }
}
