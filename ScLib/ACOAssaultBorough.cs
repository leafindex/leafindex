using System.Text;
namespace ScLib
{
    public class ACOAssaultBorough
    {
		string _boroughName;
		int _assaultCount, _wardCount;
        public int MONTHS_HELD = 24;
        int[] _monthCount;

		public ACOAssaultBorough(string boroughName, int assaultCount, int wardCount)
        {
			_boroughName = boroughName;
			_assaultCount = assaultCount;
			_wardCount = wardCount;
            _monthCount = new int[MONTHS_HELD];
            for (int i = 0; i < MONTHS_HELD; i++)
                _monthCount[i] = 0;
        }
        public string Borough { get { return _boroughName; } }
		public int Assaults { get { return _assaultCount; } }
		public int Wards { get { return _wardCount; } }

        public void IncrementAllMonths(ACOAssaultWard w)
        {
            for (int i = 0; i < MONTHS_HELD; i++)
                IncrementMonth(i, w.GetAssaultsInMonth(i));
        }
        public void IncrementMonth(int mth, int assaults)
        {
            if (mth >= 0 && mth < MONTHS_HELD)
                _monthCount[mth] += assaults;
        }
        public int GetAssaultsInMonth(int mth)
        {
            if (mth >= 0 && mth < MONTHS_HELD)
                return _monthCount[mth];
            else
                return 0;
        }
        public string MonthlyFiguresAsCsv()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < MONTHS_HELD; i++)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append(_monthCount[i]);
            }
            return sb.ToString();
        }
        public int MaxMonthlyFigure()
        {
            int result = 0;
            for (int i = 0; i < MONTHS_HELD; i++)
                if (result < _monthCount[i])
                    result = _monthCount[i];
            return result;
        }
    }
}
