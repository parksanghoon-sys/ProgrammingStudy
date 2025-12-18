https://gigong.tistory.com/95



New-SelfSignedCertificate -Type Custom -Subject "CN=flightsolution" -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.3","2.5.29.17={text}email=soletop") -KeyUsage DigitalSignature -KeyAlgorithm RSA -KeyLength 2048 -CertStoreLocation "Cert:\CurrentUser\My" -NotAfter (Get-Date).AddYears(5)
$pwd = ConvertTo-SecureString -String "8213341!@" -Force -AsPlainText 

Export-PfxCertificate -cert "Cert:\CurrentUser\My\8A1CFD6B9439E5DEA6861419C999F6733AFB9833" -FilePath "D:\Temp\flightAuth.pfx"  -Password $pwd

New-SelfSignedCertificate -DnsName "gigong.cf" -CertStoreLocation "cert:\LocalMachine\My"



이 메시지는 .NET 어셈블리에 강력한 이름(Strong Name)을 적용해야 한다는 코드 분석 경고인 것 같습니다. 'CcdControls.exe' 파일에 강력한 이름 서명이 필요하다는 내용입니다.

어셈블리에 강력한 이름을 적용하려면 다음 단계를 따르시면 됩니다:

1. 강력한 이름 키 파일(.snk) 생성하기:
    
    - Visual Studio의 개발자 명령 프롬프트에서 다음 명령 실행:
    
    ```
    sn -k CcdControls.snk
    ```
    
2. 프로젝트 파일에 강력한 이름 키 추가:
    
    - 프로젝트 파일(.csproj)에 다음 내용 추가:
    
    ```xml
    <PropertyGroup>
      <SignAssembly>true</SignAssembly>
      <AssemblyOriginatorKeyFile>CcdControls.snk</AssemblyOriginatorKeyFile>
    </PropertyGroup>
    ```
    
3. 또는 Visual Studio UI를 통해 설정:
    
    - 프로젝트 속성 → 서명 탭 → "어셈블리 서명" 체크박스 선택
    - 드롭다운에서 키 파일 선택 또는 "새로 만들기" 버튼으로 새 키 생성

이렇게 하면 어셈블리가 강력한 이름으로 서명되어 CA2210 경고가 해결될 것입니다.