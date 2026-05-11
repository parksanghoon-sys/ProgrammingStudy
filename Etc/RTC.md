*  DdsPeriodicMsgMonitor * DdsPeriodicMsgController::RegisterPeriodTmMsg(DdsMessage * pDdsMessage)
	* TM 의경우 CScheduleManager_Instance().SetTaskFunctor<DdsPeriodicMsgMonitor>("50Hz", pDdsPeriodicMsgMonitor, &DdsPeriodicMsgMonitor::Check, "TmMonitor"+pDdsMessage->GetClassID()); 를 이용하여 GetHeartbeat() 이용하여 현재 상태를 주기로 체크
* void DdsPeriodicMsgController::OverridePeriodTmMsg(DdsMessage * pDdsMessage,DdsPeriodicMsgMonitor * pDdsPeriodicMsgMonitor )
	* TC 의 경우는 &DdsMessage::DoPeriodicJob 를 이용하여 주기로 TC 메시지를 전송 