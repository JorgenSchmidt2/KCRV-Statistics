namespace ValidateServiceTests.SimpleFileCheckers.Data
{
    public class InterlabDataSimpleChecker_Data_1
    {
        public static string CheckData_Test_1_Int_Corr_Clean
            = "25\t24\n"
            + "12\t125\n"
            + "1\t2463";

        public static string CheckData_Test_2_Int_Corr_NotClean
            = "\t\n"
            + "\t\n"
            + "25\t24\n"
            + "12\t125\n"
            + "1\t2463\n";

        public static string CheckData_Test_3_Int_Incorr
            = "25\t24\n"
            + "12\t125\n"
            + "1\t2463\n"
            + "b\t5432";

        public static string CheckData_Test_4_Double_Corr_Clean
            = "25,5\t24,2\n"
            + "12,3\t125,5\n"
            + "1,1\t2463.5";

        public static string CheckData_Test_5_Double_Incorr
            = "\t\n"
            + "25,5b\t24,2\n"
            + "12,3\t125,5\n"
            + "1,1\t2463,5\n";

        public static string CheckData_Test_6_FirstColIsTemporary
            = "\t\n"
            + "1\t24,2\n"
            + "2,3\t125,5\n"
            + "3.5\t2463,5\n";

        public static string CheckData_Test_7_FirstColIsntTemporary
            = "\t\n"
            + "1\t24,2\n"
            + "5,3\t125,5\n"
            + "3.5\t2463,5\n";

        public static string CheckData_Test_8_NearDataIsEquals_FirstColumn
           = "\t\n"
           + "1\t24,2\n"
           + "5,3\t125,5\n"
           + "5.3\t2463,5\n";
    }
}