#!/usr/bin/env python3
"""Insert hero mock UI keys into i18n-full.json for all locales."""

import json
from pathlib import Path

ROOT = Path(__file__).resolve().parent
path = ROOT / "i18n-full.json"
data = json.loads(path.read_text(encoding="utf-8"))

NEW = {
    "en": {
        "hero.mock.cpu": "Ready",
        "hero.mock.gpu": "Active",
        "hero.mock.fps.title": "Gaming FPS Boost",
        "hero.mock.fps.text": "Game Mode · power plan · safe Windows tweaks",
        "hero.mock.rec.title": "Recommendations",
        "hero.mock.rec.text": "Review before apply — no automatic changes without confirmation.",
    },
    "de": {
        "hero.mock.cpu": "Bereit",
        "hero.mock.gpu": "Aktiv",
        "hero.mock.fps.title": "Gaming FPS Boost",
        "hero.mock.fps.text": "Game Mode · Energieplan · sichere Windows-Tweaks",
        "hero.mock.rec.title": "Empfehlungen",
        "hero.mock.rec.text": "Erst prüfen, dann anwenden — keine automatischen Änderungen ohne Bestätigung.",
    },
    "fr": {
        "hero.mock.cpu": "Prêt",
        "hero.mock.gpu": "Actif",
        "hero.mock.fps.title": "Boost FPS gaming",
        "hero.mock.fps.text": "Mode Jeu · plan d'alimentation · réglages Windows sûrs",
        "hero.mock.rec.title": "Recommandations",
        "hero.mock.rec.text": "Vérifiez avant d'appliquer — aucun changement automatique sans confirmation.",
    },
    "es": {
        "hero.mock.cpu": "Listo",
        "hero.mock.gpu": "Activo",
        "hero.mock.fps.title": "Impulso FPS gaming",
        "hero.mock.fps.text": "Modo Juego · plan de energía · ajustes Windows seguros",
        "hero.mock.rec.title": "Recomendaciones",
        "hero.mock.rec.text": "Revise antes de aplicar — sin cambios automáticos sin confirmación.",
    },
    "it": {
        "hero.mock.cpu": "Pronto",
        "hero.mock.gpu": "Attivo",
        "hero.mock.fps.title": "Boost FPS gaming",
        "hero.mock.fps.text": "Game Mode · piano alimentazione · tweak Windows sicuri",
        "hero.mock.rec.title": "Raccomandazioni",
        "hero.mock.rec.text": "Controlla prima di applicare — nessuna modifica automatica senza conferma.",
    },
    "pt": {
        "hero.mock.cpu": "Pronto",
        "hero.mock.gpu": "Ativo",
        "hero.mock.fps.title": "Boost FPS gaming",
        "hero.mock.fps.text": "Modo Jogo · plano de energia · ajustes Windows seguros",
        "hero.mock.rec.title": "Recomendações",
        "hero.mock.rec.text": "Revise antes de aplicar — sem alterações automáticas sem confirmação.",
    },
    "nl": {
        "hero.mock.cpu": "Gereed",
        "hero.mock.gpu": "Actief",
        "hero.mock.fps.title": "Gaming FPS-boost",
        "hero.mock.fps.text": "Game Mode · energieplan · veilige Windows-tweaks",
        "hero.mock.rec.title": "Aanbevelingen",
        "hero.mock.rec.text": "Eerst controleren voor toepassen — geen automatische wijzigingen zonder bevestiging.",
    },
    "pl": {
        "hero.mock.cpu": "Gotowy",
        "hero.mock.gpu": "Aktywny",
        "hero.mock.fps.title": "Gaming FPS Boost",
        "hero.mock.fps.text": "Tryb gry · plan zasilania · bezpieczne tweaki Windows",
        "hero.mock.rec.title": "Rekomendacje",
        "hero.mock.rec.text": "Najpierw sprawdź, potem zastosuj — bez automatycznych zmian bez potwierdzenia.",
    },
    "tr": {
        "hero.mock.cpu": "Hazır",
        "hero.mock.gpu": "Aktif",
        "hero.mock.fps.title": "Gaming FPS Artışı",
        "hero.mock.fps.text": "Oyun Modu · güç planı · güvenli Windows ayarları",
        "hero.mock.rec.title": "Öneriler",
        "hero.mock.rec.text": "Uygulamadan önce inceleyin — onay olmadan otomatik değişiklik yok.",
    },
    "ru": {
        "hero.mock.cpu": "Готов",
        "hero.mock.gpu": "Активен",
        "hero.mock.fps.title": "Gaming FPS Boost",
        "hero.mock.fps.text": "Игровой режим · план питания · безопасные настройки Windows",
        "hero.mock.rec.title": "Рекомендации",
        "hero.mock.rec.text": "Сначала проверьте, затем применяйте — без автоматических изменений без подтверждения.",
    },
    "uk": {
        "hero.mock.cpu": "Готовий",
        "hero.mock.gpu": "Активний",
        "hero.mock.fps.title": "Gaming FPS Boost",
        "hero.mock.fps.text": "Ігровий режим · план живлення · безпечні налаштування Windows",
        "hero.mock.rec.title": "Рекомендації",
        "hero.mock.rec.text": "Спочатку перевірте, потім застосуйте — без автоматичних змін без підтвердження.",
    },
    "cs": {
        "hero.mock.cpu": "Připraveno",
        "hero.mock.gpu": "Aktivní",
        "hero.mock.fps.title": "Gaming FPS Boost",
        "hero.mock.fps.text": "Herní režim · plán napájení · bezpečné tweaky Windows",
        "hero.mock.rec.title": "Doporučení",
        "hero.mock.rec.text": "Nejprve zkontrolujte, pak použijte — žádné automatické změny bez potvrzení.",
    },
    "ar": {
        "hero.mock.cpu": "جاهز",
        "hero.mock.gpu": "نشط",
        "hero.mock.fps.title": "تعزيز FPS للألعاب",
        "hero.mock.fps.text": "وضع اللعب · خطة الطاقة · إعدادات Windows آمنة",
        "hero.mock.rec.title": "توصيات",
        "hero.mock.rec.text": "راجع قبل التطبيق — لا تغييرات تلقائية بدون تأكيد.",
    },
    "zh": {
        "hero.mock.cpu": "就绪",
        "hero.mock.gpu": "活跃",
        "hero.mock.fps.title": "游戏 FPS 提升",
        "hero.mock.fps.text": "游戏模式 · 电源计划 · 安全 Windows 调整",
        "hero.mock.rec.title": "建议",
        "hero.mock.rec.text": "应用前先查看 — 未经确认不会自动更改。",
    },
    "ja": {
        "hero.mock.cpu": "準備完了",
        "hero.mock.gpu": "有効",
        "hero.mock.fps.title": "ゲーミング FPS ブースト",
        "hero.mock.fps.text": "ゲームモード · 電源プラン · 安全な Windows 調整",
        "hero.mock.rec.title": "おすすめ",
        "hero.mock.rec.text": "適用前に確認 — 確認なしの自動変更はありません。",
    },
    "ko": {
        "hero.mock.cpu": "준비됨",
        "hero.mock.gpu": "활성",
        "hero.mock.fps.title": "게이밍 FPS 부스트",
        "hero.mock.fps.text": "게임 모드 · 전원 계획 · 안전한 Windows 튜닝",
        "hero.mock.rec.title": "권장 사항",
        "hero.mock.rec.text": "적용 전 검토 — 확인 없이 자동 변경 없음.",
    },
    "hi": {
        "hero.mock.cpu": "तैयार",
        "hero.mock.gpu": "सक्रिय",
        "hero.mock.fps.title": "गेमिंग FPS बूस्ट",
        "hero.mock.fps.text": "गेम मोड · पावर प्लान · सुरक्षित Windows ट्वीक",
        "hero.mock.rec.title": "सिफारिशें",
        "hero.mock.rec.text": "लागू करने से पहले समीक्षा — बिना पुष्टि के कोई स्वचालित बदलाव नहीं।",
    },
    "sv": {
        "hero.mock.cpu": "Redo",
        "hero.mock.gpu": "Aktiv",
        "hero.mock.fps.title": "Gaming FPS-boost",
        "hero.mock.fps.text": "Spelläge · strömplan · säkra Windows-justeringar",
        "hero.mock.rec.title": "Rekommendationer",
        "hero.mock.rec.text": "Granska före tillämpning — inga automatiska ändringar utan bekräftelse.",
    },
    "ro": {
        "hero.mock.cpu": "Pregătit",
        "hero.mock.gpu": "Activ",
        "hero.mock.fps.title": "Boost FPS gaming",
        "hero.mock.fps.text": "Mod joc · plan alimentare · setări Windows sigure",
        "hero.mock.rec.title": "Recomandări",
        "hero.mock.rec.text": "Verificați înainte de aplicare — fără modificări automate fără confirmare.",
    },
    "hu": {
        "hero.mock.cpu": "Kész",
        "hero.mock.gpu": "Aktív",
        "hero.mock.fps.title": "Gaming FPS boost",
        "hero.mock.fps.text": "Játék mód · energiagazdálkodás · biztonságos Windows beállítások",
        "hero.mock.rec.title": "Ajánlások",
        "hero.mock.rec.text": "Alkalmazás előtt ellenőrizze — megerősítés nélkül nincs automatikus változás.",
    },
    "el": {
        "hero.mock.cpu": "Έτοιμο",
        "hero.mock.gpu": "Ενεργό",
        "hero.mock.fps.title": "Gaming FPS Boost",
        "hero.mock.fps.text": "Game Mode · σχέδιο ενέργειας · ασφαλείς ρυθμίσεις Windows",
        "hero.mock.rec.title": "Συστάσεις",
        "hero.mock.rec.text": "Ελέγξτε πριν την εφαρμογή — καμία αυτόματη αλλαγή χωρίς επιβεβαίωση.",
    },
    "th": {
        "hero.mock.cpu": "พร้อม",
        "hero.mock.gpu": "ใช้งาน",
        "hero.mock.fps.title": "บูสต์ FPS เกม",
        "hero.mock.fps.text": "โหมดเกม · แผนพลังงาน · ปรับแต่ง Windows อย่างปลอดภัย",
        "hero.mock.rec.title": "คำแนะนำ",
        "hero.mock.rec.text": "ตรวจสอบก่อนใช้ — ไม่มีการเปลี่ยนอัตโนมัติโดยไม่ยืนยัน",
    },
    "vi": {
        "hero.mock.cpu": "Sẵn sàng",
        "hero.mock.gpu": "Hoạt động",
        "hero.mock.fps.title": "Tăng FPS gaming",
        "hero.mock.fps.text": "Chế độ Game · kế hoạch nguồn · tinh chỉnh Windows an toàn",
        "hero.mock.rec.title": "Khuyến nghị",
        "hero.mock.rec.text": "Xem lại trước khi áp dụng — không thay đổi tự động khi chưa xác nhận.",
    },
    "id": {
        "hero.mock.cpu": "Siap",
        "hero.mock.gpu": "Aktif",
        "hero.mock.fps.title": "Boost FPS gaming",
        "hero.mock.fps.text": "Mode Game · rencana daya · tweak Windows aman",
        "hero.mock.rec.title": "Rekomendasi",
        "hero.mock.rec.text": "Tinjau sebelum terapkan — tidak ada perubahan otomatis tanpa konfirmasi.",
    },
    "da": {
        "hero.mock.cpu": "Klar",
        "hero.mock.gpu": "Aktiv",
        "hero.mock.fps.title": "Gaming FPS-boost",
        "hero.mock.fps.text": "Spilletilstand · strømplan · sikre Windows-justeringer",
        "hero.mock.rec.title": "Anbefalinger",
        "hero.mock.rec.text": "Gennemse før anvendelse — ingen automatiske ændringer uden bekræftelse.",
    },
    "no": {
        "hero.mock.cpu": "Klar",
        "hero.mock.gpu": "Aktiv",
        "hero.mock.fps.title": "Gaming FPS-boost",
        "hero.mock.fps.text": "Spillmodus · strømplan · trygge Windows-justeringer",
        "hero.mock.rec.title": "Anbefalinger",
        "hero.mock.rec.text": "Gjennomgå før bruk — ingen automatiske endringer uten bekreftelse.",
    },
    "fi": {
        "hero.mock.cpu": "Valmis",
        "hero.mock.gpu": "Aktiivinen",
        "hero.mock.fps.title": "Gaming FPS-tehostus",
        "hero.mock.fps.text": "Pelitila · virtasuunnitelma · turvalliset Windows-säädöt",
        "hero.mock.rec.title": "Suositukset",
        "hero.mock.rec.text": "Tarkista ennen käyttöä — ei automaattisia muutoksia ilman vahvistusta.",
    },
}

INSERT_AFTER = "hero.pill4"


def reorder_locale(locale: dict, extra: dict) -> dict:
    out = {}
    for k, v in locale.items():
        out[k] = v
        if k == INSERT_AFTER:
            for nk, nv in extra.items():
                out[nk] = nv
    return out


for code, table in data.items():
    if code not in NEW:
        raise SystemExit(f"missing translations for {code}")
    data[code] = reorder_locale(table, NEW[code])

path.write_text(json.dumps(data, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
print("OK: hero mock keys added to all locales")
