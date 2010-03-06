﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using ScLib;
using System.Configuration;
using System.Text;
using System.Data;

namespace Website.handler
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LondonData : IHttpHandler
    {
        private string HELP_MESSAGE = @"
            arg1 = Arts, arg2 = Public Library etc
            arg1 = Begging, arg2 = 2009 etc
            arg1 = Crimes, arg2 = Burglary etc, arg3 = 2008/09 etc";

        public void ProcessRequest(HttpContext context)
        {
            string data_set_name = context.Request["arg1"] ?? "";
            string arg2 = context.Request["arg2"] ?? "";
            string arg3 = context.Request["arg3"] ?? "";
            string fmt = context.Request["fmt"] ?? "";

            if (String.Compare(data_set_name, "Help", true) == 0)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(HELP_MESSAGE);
                return;
            }

            Mensa.SetConnectionString(ConfigurationManager.ConnectionStrings["datasets"].ToString());
            DataTable dt = new DataTable();
            if (String.Compare(data_set_name, "Crimes", true) == 0)
                dt = GetCrimeData(arg2, arg3);
            else if (String.Compare(data_set_name, "Begging", true) == 0)
                dt = GetBeggingData(arg2);
            else if (String.Compare(data_set_name, "Suicide", true) == 0)
                dt = GetSuicideData(arg2,arg3);
            else if (String.Compare(data_set_name, "Tourism", true) == 0)
                dt = GetTourismData(arg2, arg3);
            else if (String.Compare(data_set_name, "Waste", true) == 0)
                dt = GetWasteData(arg2, arg3);
            else if (String.Compare(data_set_name, "Arts", true) == 0)
                dt = GetArtsData(arg2);
            else if (String.Compare(data_set_name, "Cars", true) == 0)
                dt = GetCarsData();
            else if (String.Compare(data_set_name, "Commuters", true) == 0)
                dt = GetCommutersData();

            if( fmt.ToUpper().StartsWith( "T" ) )
                context.Response.ContentType = "text/plain";
            else
                context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Write(GetJSONString(dt));
        }

        private DataTable GetCommutersData()
        {
            DataTable result = new DataTable();
            result.Columns.Add(new DataColumn("Code", typeof(string)));
            result.Columns.Add(new DataColumn("Name", typeof(string)));
            for (int i = 0; i < 52; i++)
                result.Columns.Add(new DataColumn("Val" + i.ToString(), typeof(int)));
            foreach (string str in COMMUTER_DATA.Split('\n'))
            {
                string[] sgmts = str.Split(',');
                if (sgmts.Length < 54)
                    continue;
                if (sgmts[0].StartsWith("00"))
                {
                    DataRow dr = result.NewRow();
                    dr[0] = sgmts[1];
                    dr[1] = sgmts[0];
                    for (int i = 0; i < 52; i++)
                        dr[i + 2] = Convert.ToInt32(sgmts[i + 2]);
                    result.Rows.Add(dr);
                }
            }
            return result;
        }

        private const string COMMUTER_DATA =
        @"Origin Code,Origin Area,City of London,Barking and Dagenham,Barnet,Bexley,Brent,Bromley,Camden,Croydon,Ealing,Enfield,Greenwich,Hackney,Hammersmith and Fulham,Haringey,Harrow,Havering,Hillingdon,Hounslow,Islington,Kensington and Chelsea,Kingston upon Thames,Lambeth,Lewisham,Merton,Newham,Redbridge,Richmond upon Thames,Southwark,Sutton,Tower Hamlets,Waltham Forest,Wandsworth,Westminster,East,East Midlands,North East,North West,South East,South West,Wales,West Midlands,Yorkshire and the Humber,Northern Ireland,Scotland,Offshore,Outside UK,Residents commuting (excludes home-workers),Within Borough (includes home-workers),Within Borough (excludes home-workers),Into area (A),Out of area (B),In-Out (A-B),
00AA,City of London,1627,6,14,0,16,0,335,3,9,6,3,66,61,16,7,9,10,9,339,62,0,42,15,6,16,0,6,103,3,266,16,16,602,33,6,0,0,49,6,3,3,9,0,6,0,50,3854,2059,1627,310082,2227,307855,
00AB,Barking and Dagenham,3641,20434,194,96,178,66,1500,104,188,410,209,980,255,301,55,6415,97,91,1385,439,18,449,132,67,4460,5940,28,926,38,4069,1047,165,3280,3102,45,9,72,625,45,15,73,33,6,21,30,91,61824,24444,20434,27804,41390,-13586,
00AC,Barnet,7709,96,44045,34,5467,76,12080,148,1573,4098,101,1576,1431,3842,2623,131,1023,611,5775,1841,68,1129,124,146,387,305,229,1792,44,2511,555,536,16330,8563,254,24,142,2022,139,34,138,62,0,47,67,469,130397,59512,44045,47272,86352,-39080,
00AD,Bexley,6580,362,132,33681,144,4998,2470,710,188,123,9498,698,333,110,29,394,109,161,1557,603,77,1808,2989,190,574,111,90,4666,170,2825,222,618,7692,1107,76,20,89,9375,107,30,52,68,0,75,20,153,96084,41218,33681,26239,62403,-36164,
00AE,Brent,4145,40,6124,28,32105,66,8105,187,6703,426,124,809,3709,730,5102,51,2127,1404,2535,3926,116,1107,147,203,279,116,396,1565,56,1655,210,669,16418,3079,198,15,113,2335,140,42,144,38,9,40,100,296,107932,42991,32105,46838,75827,-28989,
00AF,Bromley,9855,134,162,3199,201,51289,3780,6268,293,84,2586,728,859,134,59,119,248,285,2094,1241,227,3570,5360,647,419,100,191,6057,796,3385,196,1371,12802,889,105,27,109,8166,148,36,147,70,0,56,58,313,128863,63942,51289,38686,77574,-38888,
00AG,Camden,8795,36,1496,32,1350,60,26535,147,643,295,140,1201,1870,666,330,43,473,473,4987,2267,89,1338,140,139,290,84,195,1784,54,2614,204,588,18829,1380,123,28,112,1257,127,33,134,43,9,42,81,419,81975,36395,26535,191077,55440,135637,
00AH,Croydon,5925,85,204,300,329,5152,3248,65033,405,120,523,599,1284,177,97,82,457,581,1752,1478,827,6844,1563,3521,202,64,480,4513,6744,2000,130,4270,10583,685,106,24,142,11854,225,41,150,69,0,56,79,360,143363,78405,65033,49714,78330,-28616,
00AJ,Ealing,4855,30,967,33,5263,91,4547,182,42004,223,136,522,8579,267,2601,48,13055,9869,1795,4111,364,1031,174,361,229,79,1578,1328,65,1420,127,944,13967,1838,169,19,143,7414,179,40,207,66,17,80,91,398,131506,54260,42004,55657,89502,-33845,
00AK,Enfield,5212,217,5642,52,1038,76,5588,136,626,44370,126,2542,850,10199,328,233,457,339,5317,1091,38,806,114,114,661,538,98,1253,47,2141,1710,314,9052,7769,110,24,120,1215,127,15,118,85,3,42,43,241,111237,54355,44370,35656,66867,-31211,
00AL,Greenwich,5614,333,181,5046,252,3196,3038,692,248,192,27753,856,607,178,45,147,154,233,1780,929,58,2292,5571,237,981,126,113,5145,134,3643,244,940,8847,893,70,21,101,3493,105,12,115,56,9,19,54,212,84965,34334,27753,30349,57212,-26863,
00AM,Hackney,5181,222,687,56,514,90,7137,128,450,877,241,18706,1165,2335,92,124,203,283,7740,1261,61,1269,240,121,1069,402,149,2023,58,5247,1101,567,9983,1017,74,32,96,694,95,28,112,52,0,24,83,142,72231,25643,18706,48551,53525,-4974,
00AN,Hammersmith and Fulham,7967,25,259,19,812,56,4009,202,2418,82,63,586,19307,137,163,33,1090,2097,1795,7072,271,1153,122,466,123,33,854,1374,113,1947,66,1908,15305,544,87,10,56,2298,140,9,75,33,0,33,79,316,75607,26683,19307,73206,56300,16906,
00AP,Haringey,5523,144,3016,32,1074,64,10506,213,668,4249,154,3130,1486,20233,268,99,330,450,7948,1889,67,1236,178,136,520,278,180,1857,48,2350,1004,507,13479,2145,132,16,111,1012,80,34,96,49,0,36,73,205,87305,28649,20233,34165,67072,-32907,
00AQ,Harrow,3162,55,5008,26,9473,47,3675,103,4542,325,62,430,1487,331,27684,29,6169,1141,1395,1160,107,501,71,119,99,44,246,854,34,1013,95,300,7882,6508,127,9,147,2967,118,23,121,63,6,39,65,232,88094,37328,27684,30119,60410,-30291,
00AR,Havering,8348,7278,194,325,156,157,2001,131,165,546,260,1302,260,335,69,39760,116,83,1932,358,33,552,137,85,3410,4844,45,1389,50,5030,1173,158,4699,9829,95,21,89,1130,88,15,91,51,0,30,24,187,97031,47260,39760,28723,57271,-28548,
00AS,Hillingdon,1661,31,692,18,2672,55,1757,116,8185,153,49,205,1799,110,4688,21,55245,5293,664,925,150,401,79,167,83,35,548,511,58,565,53,248,5062,3810,112,21,128,10141,175,63,193,64,3,49,54,254,107366,64869,55245,110568,52121,58447,
00AT,Hounslow,2342,19,253,27,807,69,1753,215,5488,92,106,250,3587,87,396,28,12803,34554,752,1800,1006,692,128,303,100,31,7025,657,101,753,41,765,5927,714,133,15,99,10320,131,18,111,33,0,47,75,256,94909,43221,34554,66025,60355,5670,
00AU,Islington,7931,110,1001,46,538,46,10188,157,479,619,138,2758,1473,1869,111,55,265,334,19946,1672,50,1191,152,131,348,117,118,1883,56,2837,393,490,12835,1087,85,22,90,880,107,7,78,90,6,19,83,229,73120,26657,19946,111643,53174,58469,
00AW,Kensington and Chelsea,10098,22,232,20,548,64,3554,165,861,48,54,445,3679,123,106,18,576,788,1816,15623,112,1029,78,208,68,30,300,1168,67,2957,68,1085,15947,440,120,19,67,1603,132,18,95,43,6,47,124,612,65283,25882,15623,75828,49660,26168,
00AX,Kingston upon Thames,2875,9,104,35,184,72,1547,638,469,42,58,176,1327,32,68,24,1070,1484,710,847,26649,1028,70,3041,71,31,3788,1038,1190,825,20,2395,5419,352,59,6,29,9666,103,30,65,34,0,30,41,258,68009,33432,26649,32950,41360,-8410,
00AY,Lambeth,9697,64,406,136,477,826,8090,3143,667,150,595,1469,3376,373,89,49,466,898,4149,4368,527,26512,1344,1830,380,123,855,8267,803,3414,185,8028,24349,715,120,35,126,2759,169,27,118,70,0,50,145,345,120784,36385,26512,76650,94272,-17622,
00AZ,Lewisham,7251,164,271,1170,332,6677,4963,2457,457,133,4555,1231,1114,277,83,93,238,365,2669,1791,181,5685,26819,583,657,141,259,11118,406,3949,236,1901,13727,744,123,30,118,2584,138,38,123,97,0,33,74,238,106293,35169,26819,29494,79474,-49980,
00BA,Merton,6051,34,181,56,251,316,2952,3193,554,49,176,454,2310,143,68,20,532,848,1411,2223,3516,3383,250,23205,157,31,1334,2282,3743,1704,81,8410,10148,379,100,18,56,5585,104,28,81,48,0,25,50,273,86813,31257,23205,34651,63608,-28957,
00BB,Newham,5009,2702,401,147,513,207,3855,190,431,584,735,2397,741,675,57,934,207,283,2709,1497,86,1154,386,139,24269,2969,126,2089,56,9120,2161,534,8565,1674,62,13,48,925,100,18,155,53,6,19,88,181,79270,31322,24269,35873,55001,-19128,
00BC,Redbridge,8205,4664,567,141,386,132,3790,173,343,1187,330,2338,581,906,110,3004,212,231,3104,746,46,853,188,100,6031,28666,88,1760,48,6916,5441,262,8122,5610,95,18,77,1006,84,30,131,60,3,52,32,213,97082,37633,28666,30880,68416,-37536,
00BD,Richmond upon Thames,4831,15,173,12,342,64,2504,323,1469,41,64,276,3181,53,167,20,3365,6873,1002,1745,3549,1216,84,840,65,12,24074,1213,260,1197,46,1991,8336,587,93,28,42,8556,187,24,107,55,3,46,44,365,79540,33926,24074,34527,55466,-20939,
00BE,Southwark,9041,108,319,281,428,933,6003,1213,536,135,1138,1401,1682,271,123,74,312,466,3203,2552,273,8500,3163,566,580,161,313,28224,261,4243,251,2545,16500,783,101,28,106,1922,142,36,102,69,9,40,85,324,99546,36537,28224,105411,71322,34089,
00BF,Sutton,2560,33,81,68,138,514,1450,7602,309,45,101,230,851,51,50,23,385,541,681,882,3122,1827,270,6725,95,26,750,1404,30814,872,41,3217,4691,385,63,18,90,11382,107,28,63,75,0,27,34,169,82890,38225,30814,28477,52076,-23599,
00BG,Tower Hamlets,10104,370,257,140,192,192,4080,183,291,224,275,2668,935,326,56,304,146,245,3235,1249,58,1090,316,124,1845,407,151,2182,55,23242,562,405,9304,1205,46,15,77,1071,100,46,83,33,0,31,71,178,68169,28900,23242,128180,44927,83253,
00BH,Waltham Forest,6337,846,715,79,491,149,5554,253,440,2661,337,3957,832,2467,112,594,206,271,4310,1378,50,1059,228,122,3257,3736,101,1719,41,4789,28062,409,10314,3696,71,9,73,792,72,21,109,61,3,15,43,191,91032,34797,28062,25782,62970,-37188,
00BJ,Wandsworth,13935,34,297,69,422,324,7140,1496,970,116,256,1044,6232,225,129,31,853,1690,3404,6208,1806,5951,403,4329,208,59,2338,4092,992,4024,135,29768,24409,752,149,27,115,5036,200,33,122,72,3,79,80,411,130468,40577,29768,57152,100700,-43548,
00BK,Westminster,9521,25,514,17,1229,99,6786,216,925,121,93,745,2369,228,142,26,597,572,2442,4767,90,1381,148,178,170,60,258,1663,61,2851,121,873,36348,792,81,30,106,1678,163,36,100,54,3,52,106,711,79548,46254,36348,463858,43200,420658,
A,North East,185,39,114,19,39,87,176,66,109,56,27,59,99,41,48,24,312,143,139,70,63,42,37,70,53,27,80,118,23,178,12,45,540,0,0,0,0,0,0,0,0,0,0,0,0,0,3140,0,0,0,0,0,
B,North West,585,22,268,55,146,189,535,170,389,117,69,133,296,145,192,65,973,391,383,177,129,180,56,132,81,106,137,296,148,253,80,130,1648,0,0,0,0,0,0,0,0,0,0,0,0,0,8676,0,0,0,0,0,
D,Yorkshire and the Humber,416,43,183,37,98,123,424,168,253,123,71,108,181,107,166,72,505,266,259,153,124,134,39,89,46,107,124,234,103,208,39,143,1327,0,0,0,0,0,0,0,0,0,0,0,0,0,6473,0,0,0,0,0,
E,East Midlands,1224,53,425,71,314,126,1370,208,362,289,91,296,344,210,236,95,961,376,708,274,169,329,75,164,125,100,162,572,149,578,120,207,2984,0,0,0,0,0,0,0,0,0,0,0,0,0,13767,0,0,0,0,0,
F,West Midlands,766,62,267,50,200,160,712,169,429,123,63,172,320,105,141,102,1080,447,328,334,134,239,78,133,115,63,162,327,643,307,57,140,2007,0,0,0,0,0,0,0,0,0,0,0,0,0,10435,0,0,0,0,0,
G,East,53219,8137,12970,1062,6399,946,20164,1089,3857,15203,1013,6376,3244,4671,8085,13757,7770,2596,13907,3504,481,3579,760,615,5926,8708,787,8629,425,19561,6746,1415,38149,0,0,0,0,0,0,0,0,0,0,0,0,0,283750,0,0,0,0,0,
H,London,311709,48238,91317,59920,78943,89975,217612,114747,97661,80026,58102,67257,92513,54398,57803,68483,165813,100579,131589,91451,59599,103162,56313,57856,60142,59546,58601,133635,59291,151422,53844,86920,500206,73106,3390,651,3089,131812,4083,911,3612,1858,104,1307,2176,9292,3754064,1282521,996586,2522087,2035804,486283,
J,South East,41843,1060,1929,13077,3117,11816,18318,16452,7769,1052,5605,3125,7793,764,2623,1211,48130,21528,8881,6282,14499,9992,3980,7297,1549,593,9414,14806,10026,13247,708,7144,59199,0,0,0,0,0,0,0,0,0,0,0,0,0,374829,0,0,0,0,0,
K,South West,1565,36,232,93,234,226,1106,243,311,176,93,174,488,111,138,71,1985,800,500,542,244,505,69,153,96,83,358,587,248,524,55,398,3799,0,0,0,0,0,0,0,0,0,0,0,0,0,16243,0,0,0,0,0,
922,Northern Ireland,40,0,31,3,6,6,42,18,12,9,0,6,15,0,6,0,129,9,12,46,12,9,3,9,6,0,6,21,15,39,3,15,146,0,0,0,0,0,0,0,0,0,0,0,0,0,674,0,0,0,0,0,
924,Wales,278,39,109,32,68,73,245,44,173,52,31,33,121,37,61,51,372,177,139,68,52,74,33,45,42,60,67,146,45,153,27,86,654,0,0,0,0,0,0,0,0,0,0,0,0,0,3687,0,0,0,0,0,
";

        private DataTable GetCarsData()
        {
            DataTable result = new DataTable();
            result.Columns.Add(new DataColumn("Code", typeof(string)));
            result.Columns.Add(new DataColumn("Name", typeof(string)));
            result.Columns.Add( new DataColumn( "Val0", typeof(int)));
            result.Columns.Add( new DataColumn( "Val1", typeof(int)));
            result.Columns.Add( new DataColumn( "Val2", typeof(int)));
            result.Columns.Add( new DataColumn( "Val3", typeof(int)));
            result.Columns.Add( new DataColumn( "Val4", typeof(int)));

result.Rows.Add( "City of London","00AA",2691,1417,184,29,17 );
result.Rows.Add( "Barking and Dagenham","00AB",25511,30279,9688,1441,354 );
result.Rows.Add( "Barnet","00AC",33925,57039,28680,5648,1652 );
result.Rows.Add( "Bexley","00AD",21217,41958,20986,4160,1130 );
result.Rows.Add( "Brent","00AE",37287,42606,16207,3135,756 );
result.Rows.Add( "Bromley","00AF",28950,57751,31154,6180,1831 );
result.Rows.Add( "Camden","00AG",50946,33084,6280,984,309 );
result.Rows.Add( "Croydon","00AH",41461,63286,27640,5066,1546 );
result.Rows.Add( "Ealing","00AJ",37372,54259,21761,3742,889 );
result.Rows.Add( "Enfield","00AK",31496,50201,22814,4586,1301 );
result.Rows.Add( "Greenwich","00AL",37883,40160,12260,1976,509 );
result.Rows.Add( "Hackney","00AM",48219,31876,5018,689,240 );
result.Rows.Add( "Hammersmith and Fulham","00AN",36630,30461,7032,1058,257 );
result.Rows.Add( "Haringey","00AP",42820,38005,9622,1374,349 );
result.Rows.Add( "Harrow","00AQ",17972,34900,20789,4286,1165 );
result.Rows.Add( "Havering","00AR",21374,42078,22131,4734,1405 );
result.Rows.Add( "Hillingdon","00AS",20972,43116,25690,5225,1640 );
result.Rows.Add( "Hounslow","00AT",24049,38920,16868,3316,841 );
result.Rows.Add( "Islington","00AU",47413,29194,4795,670,209 );
result.Rows.Add( "Kensington and Chelsea","00AW",39870,31041,6633,1184,418 );
result.Rows.Add( "Kingston upon Thames","00AX",14621,29049,14336,2669,751 );
result.Rows.Add( "Lambeth","00AY",60338,46080,10166,1446,417 );
result.Rows.Add( "Lewisham","00AZ",45941,46679,12484,1831,477 );
result.Rows.Add( "Merton","00BA",23775,38143,13803,2517,646 );
result.Rows.Add( "Newham","00BB",44866,37811,7789,1089,266 );
result.Rows.Add( "Redbridge","00BC",24198,43047,20140,3856,1047 );
result.Rows.Add( "Richmond upon Thames","00BD",18047,37723,16871,2811,694 );
result.Rows.Add( "Southwark","00BE",54940,40947,8454,1120,345 );
result.Rows.Add( "Sutton","00BF",17790,35264,18470,3750,1128 );
result.Rows.Add( "Tower Hamlets","00BG",44582,28997,4250,545,156 );
result.Rows.Add( "Waltham Forest","00BH",34975,39562,12512,2177,562 );
result.Rows.Add( "Wandsworth","00BJ",47066,51440,14437,2164,546 );
result.Rows.Add( "Westminster","00BK",51452,32108,6241,1012,359 );

            return result;
        }
        private DataTable GetSuicideData(string fact, string year)
        {
            Mensa m = new Mensa();
            return m.LondonSuicideData(fact, year).Tables[0];
        }
        private DataTable GetTourismData(string fact, string year)
        {
            Mensa m = new Mensa();
            return m.LondonTourismData(fact, year).Tables[0];
        }
        private DataTable GetWasteData(string fact, string year)
        {
            Mensa m = new Mensa();
            return m.LondonWasteData(fact, year).Tables[0];
        }


        private DataTable GetBeggingData(string year)
        {
            if (year == "") year = "2009";
            Mensa m = new Mensa();
            return m.LondonBoroughBegging(year).Tables[0];
        }

        private DataTable GetArtsData(string arttype )
        {
            if (arttype == "") arttype = "Public Library";
            Mensa m = new Mensa();
            return m.LondonArtsEngagement(arttype).Tables[0];
        }

        private void PopulateTable(DataTable dt1, string colname1, DataTable dt2, string colname2 )
        {
            foreach (DataRow dr1 in dt1.Rows)
            {
                foreach (DataRow dr2 in dt2.Rows)
                {
                    if (dr1[0].ToString() == dr2[0].ToString())
                    {
                        dr1[colname1] = dr2[colname2];
                        break;
                    }
                }
            }
        }

        private DataTable GetCrimeData( string crime, string year )
        {
            if (crime == "") crime = "Burglary";
            if (year == "") year = "2008/09";

            Mensa m = new Mensa();
            DataTable dt_all = m.LondonBoroughCrime3OffencesPerBorough(crime, year).Tables[0];
            DataTable dt = dt_all.Clone();
            foreach (DataRow dr in dt_all.Select(MakeBoroughSelect()))
            {
                DataRow dr2 = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                    dr2[i] = dr[i];
                dt.Rows.Add(dr2);
            }
            return dt;
        }

        private string MakeBoroughSelect()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string code in DistrictCodes)
            {
                if (sb.Length > 0)
                    sb.Append(" or ");
                sb.Append("[ONS Code] = '" + code + "'");
            }
            return sb.ToString();
        }

        //private string[] Boroughs = new string[] { 
        //        "City of London", "Barking and Dagenham", "Barnet", "Bexley", "Brent", "Bromley", "Camden", "Croydon", 
        //        "Ealing", "Enfield", "Greenwich", "Hackney", "Hammersmith and Fulham", "Haringey", "Harrow", 
        //        "Havering", "Hillingdon", "Hounslow", "Islington", "Kensington and Chelsea", 
        //        "Kingston upon Thames", "Lambeth", "Lewisham", "Merton", "Newham", "Redbridge", 
        //        "Richmond upon Thames", "Southwark", "Sutton", "Tower Hamlets", "Waltham Forest", 
        //        "Wandsworth", "City of Westminster"
        //};

        private string[] DistrictCodes = new string[] {
            "00AA","00AB","00AC","00AD","00AE","00AF","00AG","00AH",
            "00AJ","00AK","00AL","00AM","00AN","00AP","00AQ",
            "00AR","00AS","00AT","00AU","00AW",
            "00AX","00AY","00AZ","00BA","00BB","00BC",
            "00BD","00BE","00BF","00BG","00BH",
            "00BJ","00BK" 
        };


        public static string GetJSONString(DataTable dt)
        {
			string s;
			decimal d;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i > 0)
                    sb.Append(", ");
                sb.Append("{ ");

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string column_name;
                    if (j == 0)
                        column_name = "Name";
                    else if (j == 1)
                        column_name = "Code";
                    else
                    {
                        int figno = j - 2;
                        column_name = "Fig" + figno.ToString().PadLeft(2, '0');
                    }
					if (j > 0)
					{
						sb.Append(", ");
					}
					s = dt.Rows[i][j].ToString();
					s = (j > 1 && decimal.TryParse(s, out d)) ? Convert.ToInt32(d).ToString() : AddQuotes(s);
					sb.Append(String.Format("{0} : {1}", AddQuotes(column_name), s));
                }
                sb.AppendLine(" }");
            }
            sb.Append("]");
            return sb.ToString();
        }

        private static string AddQuotes(string value)
        {
            char dquote = '"';
            return dquote + value + dquote;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
