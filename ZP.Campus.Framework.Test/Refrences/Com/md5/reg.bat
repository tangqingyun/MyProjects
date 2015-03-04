copy md5.dll %SystemRoot%\SysWOW64 /y

regsvr32 C:\Windows\Microsoft.NET\Framework64\v4.0.30319\md5com.dll

echo "FastDataTran Install Finished"