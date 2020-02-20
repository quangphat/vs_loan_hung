using Microsoft.Extensions.Options;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VietBankApi.Business.Interfaces;
using VietBankApi.Infrastructures;
using VS_LOAN.Core.Entity.EasyCredit;

namespace VietBankApi.Business.Classes
{
    public class MediaBusiness : BaseBusiness, IMediaBusiness
    {
        protected readonly ILogBusiness _log;
        public MediaBusiness(CurrentProcess currentProcess, IOptions<ApiSetting> options, ILogBusiness logBusiness) : base(currentProcess, options)
        {
            _log = logBusiness;
        }
        private void HandleKeyEvent(object sender, AuthenticationPromptEventArgs e)
        {
            foreach (AuthenticationPrompt prompt in e.Prompts)
            {
                if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    prompt.Response = _appSetting.SftpPassword;
                }
            }
        }
        public async Task<EcResponseModel<bool>> UploadSFtp(string fileName)
        {

            try
            {
                KeyboardInteractiveAuthenticationMethod keybAuth = new KeyboardInteractiveAuthenticationMethod(_appSetting.SftpUsername);
                keybAuth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);
                var req = System.Net.WebRequest.Create(fileName);
                
                
                ConnectionInfo conInfo = new ConnectionInfo(_appSetting.SFTPPath, _appSetting.SftpPort, _appSetting.SftpUsername, keybAuth);
                using (var client = new SftpClient(conInfo))
                {
                    client.Connect();
                    if (client.IsConnected)
                    {
                        await _log.InfoLog("starting connect to ftp");
                        await _log.InfoLog("folder: ", $"/{_appSetting.SftpFolder}");
                        client.ChangeDirectory($"/{_appSetting.SftpFolder}");
                        await _log.InfoLog("starting read file: ", fileName);
                        using (Stream stream = req.GetResponse().GetResponseStream())
                        {
                            await _log.InfoLog("file info:");
                            client.BufferSize = 4 * 1024; // bypass Payload error large files
                            await _log.InfoLog("file name :", Path.GetFileName(fileName));
                            client.UploadFile(stream, Path.GetFileName(fileName));
                            await _log.InfoLog("after upload file");
                        }
                        
                        client.Disconnect();
                        return new EcResponseModel<bool>
                        {
                            error = false,
                            message = "success",
                            data = true
                        };
                    }
                    else
                    {
                        await _log.InfoLog("I couldn't connect");
                        return new EcResponseModel<bool>
                        {
                            error = true,
                            message = "cannot connect",
                            data = false
                        };
                    }
                    
                }
            }
            catch (Exception e)
            {
                await _log.InfoLog("sftp error: ", e.Message);
                return new EcResponseModel<bool>
                {
                    error = true,
                    message = e.Message,
                    data = false
                };
            }

        }
    }
}
