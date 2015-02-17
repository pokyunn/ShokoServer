; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{898530ED-CFC7-4744-B2B8-A8D98A2FA06C}
AppName=JMM Server
AppVersion=3.4.2.19
;AppVerName=JMM Server 3.4.2.19
AppPublisher=JMM
AppPublisherURL=https://github.com/japanesemediamanager
AppSupportURL=https://github.com/japanesemediamanager
AppUpdatesURL=https://github.com/japanesemediamanager
DefaultDirName={pf}\JMM Server
DefaultGroupName=JMM Server
AllowNoIcons=yes
OutputBaseFilename=JMM_Server_Setup
Compression=lzma
SolidCompression=yes

[Components]
Name: "main"; Description: "Main Files"; Types: full compact custom; Flags: fixed
Name: "firewall"; Description: "Open JMM Server port on Firewall"; Types: full compact custom;

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Components: main; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Components: main; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\JMMServer.exe"; Components: main; DestDir: "{app}"; Flags: ignoreversion 
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\Antlr3.Runtime.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\AutomaticUpdaterWPF.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\Castle.Core.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\client.wyc"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\FluentNHibernate.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\FluentNHibernate.pdb"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\HibernatingRhinos.Profiler.Appender.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\ICSharpCode.SharpZipLib.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\Iesi.Collections.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\Iesi.Collections.pdb"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\JMMContracts.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\JMMContracts.pdb"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\JMMFileHelper.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\JMMFileHelper.pdb"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\JMMServer.pdb"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\JMMServer.vshost.exe"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\JMMServer.vshost.exe.manifest"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\Microsoft.SqlServer.ConnectionInfo.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\Microsoft.SqlServer.Management.Sdk.Sfc.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\Microsoft.SqlServer.Smo.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\Microsoft.SqlServer.SqlEnum.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\MySql.Data.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\NHibernate.ByteCode.Castle.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\NHibernate.ByteCode.Castle.pdb"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\NHibernate.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\NHibernate.pdb"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\NLog.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\Remotion.Data.Linq.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\System.Data.SQLite.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\wyUpdate.exe"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\TMDbLib.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\TMDbLib.pdb"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\RestSharp.dll"; Components: main; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\Config\JMMServer.exe.Config"; Components: main; DestDir: "{app}"; Flags: ignoreversion onlyifdoesntexist
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\x64\hasher.dll"; Components: main; DestDir: "{app}\x64"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\x64\MediaInfo.dll"; Components: main; DestDir: "{app}\x64"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\x64\SQLite.Interop.dll"; Components: main; DestDir: "{app}\x64"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\x86\hasher.dll"; Components: main; DestDir: "{app}\x86"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\x86\MediaInfo.dll"; Components: main; DestDir: "{app}\x86"; Flags: ignoreversion
Source: "C:\Projects\[ JMM Binaries No Configs ]\JMMServer\x86\SQLite.Interop.dll"; Components: main; DestDir: "{app}\x86"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\JMM Server"; Components: main; Filename: "{app}\JMMServer.exe"
Name: "{group}\{cm:UninstallProgram,JMM Server}"; Components: main; Filename: "{uninstallexe}"
Name: "{commondesktop}\JMM Server"; Components: main; Filename: "{app}\JMMServer.exe"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\JMM Server"; Components: main; Filename: "{app}\JMMServer.exe"; Tasks: quicklaunchicon

[Run]
Filename: "{sys}\netsh.exe"; Parameters: "advfirewall firewall add rule name=""JMM Server - Client Port"" dir=in action=allow protocol=TCP localport=8111"; StatusMsg: "Open exception on firewall..."; Flags: runhidden; Components:firewall;
Filename: "{app}\JMMServer.exe"; Description: "{cm:LaunchProgram,JMM Server}"; Flags: nowait postinstall skipifsilent

[UninstallRun]
Filename: "{sys}\netsh.exe"; Parameters: "advfirewall firewall delete rule name=""JMM Server - Client Port"" protocol=TCP localport=8111"; StatusMsg: "Open exception on firewall..."; Flags: runhidden; Components:firewall;

