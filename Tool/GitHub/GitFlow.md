
# Git flow
## git flow 전략
---
* 브런치로 구분해서 전략을 해야한다.
* 예시
* 기존 개발 `main` 브런치 0.9
* 신기능을 개발 시 `develop` 브런치로 신기능을 추가 한다. 다양한 기능일시
* `feature` 브런치를 만들어 개별로 한개씩 신기능을 개발한다. 다음 완성시 `develop에` 합친다.
* `develop` 브런치가 완성시 `release` 브런치를 만들어 출시전 테스트를 한다.
* 테스트 완료시 `main` 브런치에 다시합친다
* main 브런치를 배포 한다
* 배포시 버그 생성시 `hotfix` 브런치를 따서 수정후 `main` 에 합친다.

## Trunck_bases 전략
---
* main 브런치 하나로 개발한다.
* 수정이 필요할시 feature 브런치를 만들어서 개발후 main에 합친다.
* 테스트를 많이 해야한다. 
=======
# Git flow
## git flow 전략
---
* 브런치로 구분해서 전략을 해야한다.
* 예시
* 기존 개발 `main` 브런치 0.9
* 신기능을 개발 시 `develop` 브런치로 신기능을 추가 한다. 다양한 기능일시
* `feature` 브런치를 만들어 개별로 한개씩 신기능을 개발한다. 다음 완성시 `develop에` 합친다.
* `develop` 브런치가 완성시 `release` 브런치를 만들어 출시전 테스트를 한다.
* 테스트 완료시 `main` 브런치에 다시합친다
* main 브런치를 배포 한다
* 배포시 버그 생성시 `hotfix` 브런치를 따서 수정후 `main` 에 합친다.

## Trunck_bases 전략
---
* main 브런치 하나로 개발한다.
* 수정이 필요할시 feature 브런치를 만들어서 개발후 main에 합친다.
* 테스트를 많이 해야한다. 

[[GitHub명령어]]
>>>>>>> ae3be484e8d676e9969f98b0ea229e3c4f51d0fb
