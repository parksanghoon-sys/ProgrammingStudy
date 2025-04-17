# Git Branch
## Git branch 명명 규칙
---
* **master** : main branch
  * 제품으로 출시 될 수 있는 브런치
  * realse 이력을 관리하기 위해 사용. 즉, 배포 가능한 상태만을 관리
* **develop** : main branch
  * 다음 버전을 출시하기 위한 브런치
  * 기능이 추가되고 버그가 수정되어 배포가능한 상태라면 develop브런치를 'master'브런치에 병합한다
* **feature/{feature-name}** : <Issue_number> or <Feature_name>\<Short Description>
  * 기능을 개발하는 브런치
  * 새로은 기능 개발 및 버그 수정이 필요시마다 'develop' 브런치로 부터 분기한다.
  * feature 브런치에서의 작업은 공유할 필요가 없어 자신의 로컬에 저장한다.
  * 개발 완료시 'develop' 브런치로 병합하여 공유한다.
  * 더 이상 필요 하지 않는 feature 브런치는 삭제한다.
  * feature 이름정하기
    * feature/기능요약 형식을 추천 ex) feature/login
* **bugfix/(bug-name)** :버그 수정을 위한 브랜치
  * 현재 프로잭트의 버그를 수정하기 위한 브런치
* **release** : Verseon
  * 현재 출시 버전을 준비하는 브런치
  * 배포를 위한 전용 브런치를 사용함으로써 한 팀이 해당 배포를 준비하는동안 다른팀은 다른 기능개발을 계속 할 수 있다.
  * 순서
    1. 'develop' 브런치에 배포 수준의 기능이 모일시 'release' 브런치를 분기한다.
    2. 직접적으로 관련된 작업을 제외하고 release 브런치에 새로운 기능을 추가 병합하지 않는다.
    3. 'release' 브런치에서 배포 가능 상태시 'master' 브런치에 병합 한다. (이떄, 병합한 커밋에 Release버전 태그를 부여)
    4. 배포를 준비하는 동안 release 브런치가 변경될수 있으므로 배포 완료후 'develop'브런치에도 병합한다.
   * 이름 정하기
     * release-* 형식을 사용 ex) release-1.2
* **hotfix** :  <Issue_number>
  * 출시 버전 에서 발생한 버그를 수정하는 브런치
  * 배포 버젼에서 긴급하게 수정을 해야할때 'master' 브런치에서 분기하는 브런치이다.
  * 긴급 버그 수정을 위한 브런치 
  * 순서
    1. 배포 버젼에 긴급 수정 필요가 생길경우 'master' 브런치에서 'hotfix' 브런치를 분기한다.
    2. 문제가 되는부분을 빠르게 수정한다
    3. 다시 'master'브런치에 병합
    4. 새로운 버전 이름으로 태그를 단다.
    5. hotfix 브런치의 변경사항을 'develop' 브런치에도 병합한다.    