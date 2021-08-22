using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;

namespace Mvc_Clinic.Models
{
    public class LoginLogModel
    {
        public int Choice { get; set; }
        public int LoginID { get; set; }

        public string Ipaddress { get; set; }

        public LoginLogModel(int ch,int AdminID)
        {
            this.Choice = ch;
            this.LoginID = AdminID;
            string ip = getIPAddress();
            this.Ipaddress = ip;
        }


        public static string getIPAddress()
        {
            string szRemoteAddr = HttpContext.Current.Request.UserHostAddress;
            string szXForwardedFor = HttpContext.Current.Request.ServerVariables["X_FORWARDED_FOR"];
            if (szXForwardedFor == "" || szXForwardedFor == null)
                szXForwardedFor = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            string szIP = "";

            if (szXForwardedFor == null)
            {
                szIP = szRemoteAddr;
            }
            else
            {
                szIP = szXForwardedFor;
                if (szIP.IndexOf(",") > 0)
                {
                    string[] arIPs = szIP.Split(',');

                    foreach (string item in arIPs)
                    {
                        if (!IsPrivateIpAddress(item))
                        {
                            return item;
                        }
                    }
                }
            }
            return szIP;
        }

        private static bool IsPrivateIpAddress(string ipAddress)
        {
            // http://en.wikipedia.org/wiki/Private_network
            // Private IP Addresses are: 
            //  24-bit block: 10.0.0.0 through 10.255.255.255
            //  20-bit block: 172.16.0.0 through 172.31.255.255
            //  16-bit block: 192.168.0.0 through 192.168.255.255
            //  Link-local addresses: 169.254.0.0 through 169.254.255.255 (http://en.wikipedia.org/wiki/Link-local_address)

            var ip = IPAddress.Parse(ipAddress);
            var octets = ip.GetAddressBytes();

            var is24BitBlock = octets[0] == 10;
            if (is24BitBlock) return true; // Return to prevent further processing

            var is20BitBlock = octets[0] == 172 && octets[1] >= 16 && octets[1] <= 31;
            if (is20BitBlock) return true; // Return to prevent further processing

            var is16BitBlock = octets[0] == 192 && octets[1] == 168;
            if (is16BitBlock) return true; // Return to prevent further processing

            var isLinkLocalAddress = octets[0] == 169 && octets[1] == 254;
            return isLinkLocalAddress;
        }

    }

}