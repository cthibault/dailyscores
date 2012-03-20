using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace RequestBuilder
{
    class Program
    {
        private static readonly Dictionary<string, bool> _tokens = new Dictionary<string, bool>
                                                                   {
                                                                       { "S", true },
                                                                       { "SUBMIT", true },
                                                                       { "E", false },
                                                                       { "EXIT", false },
                                                                   };

        static void Main(string[] args)
        {
            //Console.Write("Enter Port Address: ");
            //string port = Console.ReadLine();
            string port = "10824";

            do
            {
                Console.WriteLine(":: BEGIN REQUEST ::");
                SubmitRequest(port);
                Console.WriteLine("::  END REQUEST  ::");
            } while (GetInput());
        }
        
        private static bool GetInput()
        {
            bool result = false;
            bool isValid = false;

            while (!isValid)
            {
                Console.Write("(S)ubmit -or- (E)xit: ");
                string input = (Console.ReadLine() ?? "EXIT").ToUpper();

                isValid = _tokens.ContainsKey(input);
                if (isValid)
                {
                    result = _tokens[input];
                }
            }

            return result;
        }

        private static void SubmitRequest(string port)
        {
            var request = WebRequest.Create(string.Format("http://localhost:{0}/Scores/EmailSubmission", port));
            request.Method = "POST";

            string postData = "[ [ \"Received\", \"by luna.mailgun.net with SMTP mgrt -1061595300; Wed, 14 Mar 2012 20:57:46 +0000\" ], [ \"X-Envelope-From\", \"<curtistbone@gmail.com>\" ], [ \"Received\", \"from mail-pz0-f45.google.com (mail-pz0-f45.google.com [209.85.210.45]) by mxa.mailgun.org with ESMTP id 4f61064a.4b354f0-luna; Wed, 14 Mar 2012 20:57:46 -0000 (UTC)\" ], [ \"Received\", \"by dadp14 with SMTP id p14so3217303dad.4\r for <scores@dailyscores.mailgun.org>; Wed, 14 Mar 2012 13:57:45 -0700 (PDT)\" ], [ \"Dkim-Signature\", \"v=1; a=rsa-sha256; c=relaxed/relaxed;\r d=gmail.com; s=20120113;\r h=mime-version:sender:date:x-google-sender-auth:message-id:subject\r :from:to:content-type;\r bh=bJN/zKu15/5ug/7vlAPYb5R5MFaAUI2JtSUrEvkXmH4=;\r b=kI6QjLL271sEg8u7C252PKZhL31UWK4PkT4esxNODHqzhNxt7LhfYH+hcA02cxNJS6\r V8tqpEiO3e5FJbw6tdyQNsWxIGBsI2AtlCT3z7uav4pE4JWkVB9HXUGpo/Bb1DtfNoLC\r 4UEBxmgFUUGrLBzw0jgXJ3dgYJr4ZCSDq/uc75Hx9ET7HuuHogE9RMkwrMwrzxynIuir\r jtAzAdH2dazWto5QeTYpUS68nuA7G13jgM/yYgeElAWPlzd/Upqd1EY/zzcu+cL4pF32\r vx94RFYQ/MBHLLra0OQJsfaeQQtvkt6+chvMrghku0ePPpUpmROjXmSwL1i1x5kxelmg\r g7Ug==\" ], [ \"Received\", \"by 10.68.193.138 with SMTP id ho10mr4732541pbc.80.1331758665363;\r Wed, 14 Mar 2012 13:57:45 -0700 (PDT)\" ], [ \"Sender\", \"curtistbone@gmail.com\" ], [ \"Received\", \"by 10.68.194.74 with HTTP; Wed, 14 Mar 2012 13:57:45 -0700 (PDT)\" ], [ \"Date\", \"Wed, 14 Mar 2012 15:57:45 -0500\" ], [ \"X-Google-Sender-Auth\", \"U094IT7IMtdvXyMMhK7UP2YNJkc\" ], [ \"Message-Id\", \"<CAL3OhavS7RWK8Wo72x8YFakx_DOWGJem1OK3jx+Ni4Og3gtG9w@mail.gmail.com>\" ], [ \"Subject\", \"Test Subject\" ], [ \"From\", \"Curtis Thibault <curtis.thibault@gmail.com>\" ], [ \"To\", \"scores <scores@dailyscores.mailgun.org>\" ] ]";
            byte[] postDataBArray = Encoding.UTF8.GetBytes(postData);

            

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataBArray.Length;

            var stream = request.GetRequestStream();
            if (stream != null)
            {
                stream.Write(postDataBArray, 0, postDataBArray.Length);
                stream.Close();

                
                try
                {
                    var response = request.GetResponse();
                    Console.WriteLine(((HttpWebResponse) response).StatusDescription);

                    stream = response.GetResponseStream();
                    if (stream != null)
                    {
                        var reader = new StreamReader(stream);
                        string responseFromServer = reader.ReadToEnd();
                        Console.WriteLine(responseFromServer);

                        reader.Close();
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
