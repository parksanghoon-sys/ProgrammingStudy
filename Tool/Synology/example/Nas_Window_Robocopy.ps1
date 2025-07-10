# 원본 및 대상 경로 설정
$source = "\\192.168.20.52\hoon\01.Study\99.TestProject\01.Web"
$destination = "D:\Temp\Nas_Test"

# 로그 파일 경로
$logFile = "C:\Temp\robocopy_log.txt"

# Robocopy 인자 설정
$robocopyArgs = @(
    $source
    $destination
    "/MIR"           # 대상 폴더를 원본과 동일하게 맞춤 (주의: 삭제 동기화 포함)
    "/MT:16"         # 멀티스레드 16개 사용
    "/R:3"           # 실패 시 재시도 3회
    "/W:5"           # 재시도 간격 5초
    "/LOG:$logFile"  # 로그 파일 경로
)

# 복사 시작
Write-Host "복사 시작..."
robocopy @robocopyArgs
Write-Host "복사 완료!"
