# Oh-Tech

온라인 포인트 적립을 위한 자동화 도구

## 프로젝트 개요

Oh-Tech는 다양한 온라인 서비스에서 포인트 적립을 자동화하는 Windows 데스크톱 애플리케이션입니다. 사용자 친화적인 GUI를 통해 비개발자도 쉽게 자동화 기능을 활용할 수 있도록 설계되었습니다.

## 개발 환경

- **언어**: C#
- **프레임워크**: .NET 8
- **UI**: WPF (Windows Presentation Foundation)
- **플랫폼**: Windows

## 기술 스택

- **Prism**: MVVM 패턴 및 모듈화 구조 구현
- **CommunityToolkit**: 개발 생산성 향상을 위한 유틸리티
- **CefSharp**: 웹뷰 자동화 기능 구현
- **Custom Control**: Image, Place Holder 기능을 포함한 Button 제작
- **REST API**: 서버와의 데이터 통신

## 사용 도구

- **SQLite**: 클라이언트 데이터 베이스
- **NSIS**: 설치 파일 생성

## 주요 기능

### 1. 게스트 모드 및 포인트 적립 시스템
- 사용자 등록 없이 바로 사용 가능한 게스트 모드 제공
- 포인트 적립 구조를 통한 사용자 참여 유도 시스템 구축
- 직관적인 포인트 현황 표시

### 2. 스크립트 기반 자동화
- 화면 제어용 스크립트 실행 기능
- 웹 브라우저 자동화를 통한 반복 작업 처리
- 비개발자도 쉽게 사용할 수 있는 단순화된 GUI 제공

### 3. 시스템 통합
- **트레이 아이콘**: 백그라운드 실행 및 빠른 접근
- **자동 업데이트**: 최신 버전 자동 확인 및 업데이트
- **설치 파일**: NSIS를 이용한 전문적인 설치 프로그램 제공

## 주요 성과

1. **사용자 참여 유도 시스템 구축**
   - 게스트 모드와 포인트 적립 구조 설계
   - 사용자의 지속적인 참여를 유도하는 보상 시스템

2. **접근성 향상**
   - 복잡한 자동화 기능을 단순한 GUI로 구현
   - 비개발자도 쉽게 활용할 수 있는 사용자 경험 제공

3. **안정적인 배포 환경**
   - 자동 업데이트 시스템으로 유지보수 효율성 증대
   - 전문적인 설치 프로그램으로 사용자 편의성 향상


## 설치 및 실행

1. 릴리스 페이지에서 최신 설치 파일 다운로드
2. NSIS 설치 파일 실행
3. 설치 완료 후 프로그램 실행
4. 게스트 모드로 바로 시작하거나 계정 생성

## 업데이트

프로그램이 자동으로 업데이트를 확인하고 새 버전이 있을 때 알림을 표시합니다. 원클릭으로 최신 버전으로 업데이트할 수 있습니다.

## UI
1. MAIN   
   <img width="20%" height="20%" src="https://github.com/foryouself83/Oh-Cash-public/blob/master/Main.png?raw=true"/>   
2. Mission List   
   <img width="20%" height="20%" src="https://github.com/foryouself83/Oh-Cash-public/blob/master/MissionList.png?raw=true"/>   
3. PointHistory   
   <img width="20%" height="20%" src="https://github.com/foryouself83/Oh-Cash-public/blob/master/PointHistory.png?raw=true"/>
3. TrayIcon   
   <img width="20%" height="20%" src="https://github.com/foryouself83/Oh-Cash-public/blob/master/TrayIcons.png?raw=true"/>   
