using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressVerification
{
    class Program
    {
        static void Main(string[] args)
        {
            var mom365Addresses = LoadMom365Addresses();
            VerifyWithCdyne(Enumerable.Take(mom365Addresses, 1000));

            Console.WriteLine("DPV - Y :" + mom365Addresses.Count(a => a.cdyneReturnCode == 100));
            Console.WriteLine("DPV - N :" + mom365Addresses.Count(a => a.cdyneReturnCode == 101));
            Console.WriteLine("DPV - S :" + mom365Addresses.Count(a => a.cdyneReturnCode == 102));
            Console.WriteLine("DPV - D :" + mom365Addresses.Count(a => a.cdyneReturnCode == 103));

            Cdyne.PavServiceClient PSC = new Cdyne.PavServiceClient("pavsoap");

            #region Advanced Address Verification
            Cdyne.PavRequest PR = new Cdyne.PavRequest();

            PR.LicenseKey = "0";
            PR.FirmOrRecipient = "CDYNE Corporation";
            PR.PrimaryAddressLine = "505 Independence Parkway";
            PR.SecondaryAddressLine = "Suite 300";
            PR.CityName = "Chesapeake";
            PR.State = "VA";
            PR.ZipCode = "23320";
            PR.Urbanization = "";
            PR.ReturnCaseSensitive = true;
            PR.ReturnCensusInfo = true;
            PR.ReturnCityAbbreviation = true;
            PR.ReturnGeoLocation = true;
            PR.ReturnLegislativeInfo = true;
            PR.ReturnMailingIndustryInfo = true;
            PR.ReturnResidentialIndicator = true;
            PR.ReturnStreetAbbreviated = true;
 
            Cdyne.PavResponse PavRes = new Cdyne.PavResponse();
            PavRes = PSC.VerifyAddressAdvanced(PR);
            
            /* The return section below will output the return code with the return code text along with the address information.  
            To view a list of all possible return codes: http://wiki.cdyne.com/index.php/PAVv3_Return_Codes*/
 
            if (PavRes.ReturnCode == 100)
 
                Console.WriteLine(
                    "Return Code: " + PavRes.ReturnCode + " - Input address is DPV confirmed for all components." + "\n" +
                    PavRes.FirmOrRecipient + "\n" +
                    PavRes.PrimaryDeliveryLine + "\n" +
                    PavRes.CityName + ", " +
                    PavRes.StateAbbreviation + "  " +
                    PavRes.ZipCode + "\n" +
                    PavRes.Country + "\n" +
                    "DPV confirmation indicator: " + PavRes.MailingIndustryInfo.DpvConfirmationIndicator);
 
            else Console.WriteLine(
                    "Return Code: " + PavRes.ReturnCode + "\n" +
                    PavRes.FirmOrRecipient + "\n" +
                    PavRes.PrimaryDeliveryLine + "\n" +
                    PavRes.CityName + ", " +
                    PavRes.StateAbbreviation + "  " +
                    PavRes.ZipCode + "\n" +
                    PavRes.Country);
 
            Console.ReadLine();
            #endregion

        }

        private static void VerifyWithCdyne(IEnumerable<Mom365Address> Mom365Addresses)
        {
            Cdyne.PavServiceClient pavServiceClient = new Cdyne.PavServiceClient("pavsoap");

            foreach (var address in Mom365Addresses)
            {
                Cdyne.PavRequest pavRequest = new Cdyne.PavRequest();

                pavRequest.LicenseKey = "0"; //"YOUR LICENSE KEY";
                pavRequest.FirmOrRecipient = ""; //"CDYNE Corporation";
                pavRequest.PrimaryAddressLine = address.address1;
                pavRequest.SecondaryAddressLine = address.address2;
                pavRequest.CityName = address.city;
                pavRequest.State = address.state;
                pavRequest.ZipCode = address.zip;
                pavRequest.Urbanization = "";
                pavRequest.ReturnCaseSensitive = true;
                pavRequest.ReturnCensusInfo = true;
                pavRequest.ReturnCityAbbreviation = true;
                pavRequest.ReturnGeoLocation = true;
                pavRequest.ReturnLegislativeInfo = true;
                pavRequest.ReturnMailingIndustryInfo = true;
                pavRequest.ReturnResidentialIndicator = true;
                pavRequest.ReturnStreetAbbreviated = true;

                Cdyne.PavResponse pavResponse = new Cdyne.PavResponse();
                pavResponse = pavServiceClient.VerifyAddressAdvanced(pavRequest);

                address.cdyneReturnCode = pavResponse.ReturnCode;
                /* The return section below will output the return code with the return code text along with the address information.  
                To view a list of all possible return codes: http://wiki.cdyne.com/index.php/PAVv3_Return_Codes*/

                //if (pavResponse.ReturnCode == 100)

                //    Console.WriteLine(
                //        "Return Code: " + pavResponse.ReturnCode + " - Input address is DPV confirmed for all components." + "\n" +
                //        pavResponse.FirmOrRecipient + "\n" +
                //        pavResponse.PrimaryDeliveryLine + "\n" +
                //        pavResponse.CityName + ", " +
                //        pavResponse.StateAbbreviation + "  " +
                //        pavResponse.ZipCode + "\n" +
                //        pavResponse.Country);

                //else Console.WriteLine(
                //        "Return Code: " + pavResponse.ReturnCode + "\n" +
                //        pavResponse.FirmOrRecipient + "\n" +
                //        pavResponse.PrimaryDeliveryLine + "\n" +
                //        pavResponse.CityName + ", " +
                //        pavResponse.StateAbbreviation + "  " +
                //        pavResponse.ZipCode + "\n" +
                //        pavResponse.Country);

                //Console.ReadLine();
            }
        }

        private static IList<Mom365Address> LoadMom365Addresses()
        {
            var fileLocation = @"C:\Address verification\";
            var rejectsFile = "Mom365 Addresses for Test.csv";

            IList<Mom365Address> addresses = new List<Mom365Address>();

            try
            {
                using (StreamReader sr = new StreamReader(fileLocation + rejectsFile))
                {
                    String line = sr.ReadLine();
                    Console.WriteLine(line);
                    while ((line = sr.ReadLine()) != null)
                    {
                        addresses.Add(new Mom365Address(line));
                        Console.WriteLine(line);
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            return addresses;
        }
    }
}
