:: Compile application.
@%SystemRoot%\Microsoft.NET\Framework\v2.0.50727\jsc.exe /r:bin\Alibaba.F2E.JSEventEmitter.dll /out:bin\test.exe src\Test.js

:: Run test case.
@bin\test.exe

:: Clean up.
@del bin\test.exe