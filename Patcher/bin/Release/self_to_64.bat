copy /B /Y patcher2.exe patcher2_64.exe
>  self_to_64.ini echo Patcher2 32bit to 64bit
>> self_to_64.ini echo patcher2_64.exe
>> self_to_64.ini echo 03 00 02 00 30
>> self_to_64.ini echo 0
>> self_to_64.ini echo 1 ? 0
patcher2 -e self_to_64.ini
del patcher2_64.exe.bak
del self_to_64.ini
