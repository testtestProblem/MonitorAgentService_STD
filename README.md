How to install
1. start CMD in administrator
2. cd C:\Windows\Microsoft.NET\Framework\v4.0.30319
3. InstallUtil.exe “path abcd.exe" -> abcd is your .exe name
4. net start abcd  -> start
5. net stop abcd  -> stop

uninstall service
sc delete <service-name>
