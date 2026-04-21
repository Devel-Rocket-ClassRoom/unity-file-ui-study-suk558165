public enum Languages // 지원하는 언어 종류를 정의하는 열거형
{
    Korean, // 한국어
    English, // 영어
    Japanese, // 일본어
}
public enum ItemTytpes // 아이템 종류를 정의하는 열거형
{
    Weapon, // 무기 타입
    Equip, // 장비 타입
    Consumable, // 소비 아이템 타입
}
public static class Varuables // 게임 전역에서 공유하는 전역 변수 보관 정적 클래스
{
    public static Languages language = Languages.Korean; // 현재 선택된 언어 (기본값: 한국어)
}

public static class DatableIds // 데이터 테이블 파일명 상수를 관리하는 정적 클래스
{
    public static readonly string[] StringTableIds = // 언어별 스트링 테이블 파일명 배열 (Languages 열거형 순서와 일치)
    {
        "StringTableKr", // 한국어 스트링 테이블 파일명
        "StringTableEn", // 영어 스트링 테이블 파일명
        "StringTableJp", // 일본어 스트링 테이블 파일명
    };

    public static string String => StringTableIds[(int)Varuables.language]; // 현재 언어에 해당하는 스트링 테이블 파일명 반환

    public static  string Item => "ItemTable"; // 아이템 테이블 파일명 반환

    public const string Character = "CharacterTable"; // 캐릭터 테이블 파일명 상수
}
