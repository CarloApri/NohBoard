; Installer for NohBoard.
;
; This installs per user, into the local application data folder, the way VS Code and Discord do. That keeps the
; installation and every future update free of an administrator prompt, which is what makes an unattended update
; possible at all. Settings and keyboards live in the roaming profile and are deliberately left alone on upgrade.
;
; Build with:  ISCC.exe installer\NohBoard.iss
; Expects the application to have been published to publish-net8 first.

#define AppName        "NohBoard"
#define AppPublisher   "ThoNohT"
#define AppUrl         "https://github.com/ThoNohT/NohBoard"
#define PayloadDir     "..\publish-net8"
#define KeyboardsDir   "..\keyboards"

; The version is read from the executable about to be packaged, which takes it from MinVer, which takes it from
; the version control tag. One source for the whole chain, so the name of the setup, the entry in installed apps
; and the version inside the program cannot drift apart. Pass /DAppVersion=x.y.z to override.
#ifndef AppVersion
  #define ExeVersion GetVersionNumbersString(AddBackslash(SourcePath) + PayloadDir + "\NohBoard.exe")
  #define AppVersion Copy(ExeVersion, 1, RPos(".", ExeVersion) - 1)
#endif

[Setup]
; Keep this GUID stable forever, it is what lets a new version replace the previous one in place.
AppId={{6F3A1B94-6C1E-4C33-9E0E-2B7A1D5C8E41}
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
AppPublisher={#AppPublisher}
AppSupportURL={#AppUrl}
AppUpdatesURL={#AppUrl}/releases
DefaultDirName={localappdata}\Programs\{#AppName}
DefaultGroupName={#AppName}
DisableProgramGroupPage=yes
DisableDirPage=auto
PrivilegesRequired=lowest
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
OutputDir=..\dist
OutputBaseFilename={#AppName}-{#AppVersion}-setup
UninstallDisplayIcon={app}\NohBoard.exe
SetupIconFile=..\NohBoard\NohBoard\NohBoard2.ico
UninstallDisplayName={#AppName} {#AppVersion}
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
; NohBoard installs a global keyboard hook, so a running copy has to go before its files can be replaced.
CloseApplications=yes
CloseApplicationsFilter=NohBoard.exe
RestartApplications=no

[Languages]
Name: "italian"; MessagesFile: "compiler:Languages\Italian.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[InstallDelete]
; The copy of the definitions in the program folder is a read only reference that the program merges into the
; roaming profile. Clearing it first means a definition dropped from a later release does not linger here and get
; merged back in forever. User data lives in the roaming profile and is never touched by this.
Type: filesandordirs; Name: "{app}\keyboards"

[Files]
Source: "{#PayloadDir}\NohBoard.exe";           DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\NohBoard.Hooking.dll";   DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\clipper_library.dll";    DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\*.deps.json";            DestDir: "{app}"; Flags: ignoreversion
; The definitions shipped here stay read only reference copies. NohBoard merges the new ones into the roaming
; profile on first start after an update, without touching any definition that has been edited.
Source: "{#KeyboardsDir}\*"; DestDir: "{app}\keyboards"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\{#AppName}"; Filename: "{app}\NohBoard.exe"
Name: "{autodesktop}\{#AppName}";  Filename: "{app}\NohBoard.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\NohBoard.exe"; Description: "{cm:LaunchProgram,{#AppName}}"; Flags: nowait postinstall skipifsilent

[Code]
{ NohBoard is published framework dependent, so the .NET Desktop Runtime has to be present. }
function HasDesktopRuntime: Boolean;
var
  FindRec: TFindRec;
  BasePath, FolderName: String;
  DotPosition, MajorVersion: Integer;
begin
  Result := False;
  BasePath := ExpandConstant('{commonpf64}\dotnet\shared\Microsoft.WindowsDesktop.App');
  if not DirExists(BasePath) then
    Exit;

  if FindFirst(BasePath + '\*', FindRec) then
  begin
    try
      repeat
        if (FindRec.Attributes and FILE_ATTRIBUTE_DIRECTORY) <> 0 then
        begin
          FolderName := FindRec.Name;
          DotPosition := Pos('.', FolderName);
          if DotPosition > 1 then
          begin
            MajorVersion := StrToIntDef(Copy(FolderName, 1, DotPosition - 1), 0);
            if MajorVersion >= 8 then
              Result := True;
          end;
        end;
      until not FindNext(FindRec);
    finally
      FindClose(FindRec);
    end;
  end;
end;

function InitializeSetup: Boolean;
var
  ErrorCode: Integer;
begin
  Result := True;
  if not HasDesktopRuntime then
  begin
    if MsgBox('NohBoard richiede il .NET Desktop Runtime 8 (o superiore), che non risulta installato.'#13#10#13#10 +
              'Vuoi aprire la pagina di download? L''installazione verra'' annullata.',
              mbConfirmation, MB_YESNO) = IDYES then
      ShellExec('open', 'https://dotnet.microsoft.com/download/dotnet/8.0/runtime',
                '', '', SW_SHOW, ewNoWait, ErrorCode);
    Result := False;
  end;
end;

{ Settings and keyboards are user data and survive an uninstall unless the user asks otherwise. }
procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var
  DataPath: String;
begin
  if CurUninstallStep = usPostUninstall then
  begin
    DataPath := ExpandConstant('{userappdata}\NohBoard');
    if DirExists(DataPath) then
      if MsgBox('Vuoi rimuovere anche le impostazioni e le tastiere personalizzate?'#13#10#13#10 + DataPath,
                mbConfirmation, MB_YESNO) = IDNO then
        Exit
      else
        DelTree(DataPath, True, True, True);
  end;
end;
