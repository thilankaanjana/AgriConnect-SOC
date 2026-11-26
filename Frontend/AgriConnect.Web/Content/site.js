function openModal(name) {
    closeModals();
    var m = document.getElementById('modal-' + name);
    if (!m) return;
    m.classList.add('open');
    var f = m.querySelector('input[type=text],input[type=email],input[type=password],input.otp');
    if (f) setTimeout(function () { try { f.focus(); } catch (e) { } }, 60);
}
function closeModals() {
    var a = document.querySelectorAll('.overlay.open');
    for (var i = 0; i < a.length; i++) a[i].classList.remove('open');
}
function applyRole(seg, role) {
    var bs = seg.querySelectorAll('button');
    for (var i = 0; i < bs.length; i++) {
        if (bs[i].getAttribute('data-role') === role) bs[i].classList.add('on'); else bs[i].classList.remove('on');
    }
    var hid = document.getElementById(seg.getAttribute('data-hidden'));
    if (hid) hid.value = role;
}
function setRole(btn) { applyRole(btn.parentNode, btn.getAttribute('data-role')); }
function initSegs() {
    var segs = document.querySelectorAll('.seg');
    for (var i = 0; i < segs.length; i++) {
        var hid = document.getElementById(segs[i].getAttribute('data-hidden'));
        applyRole(segs[i], (hid && hid.value) ? hid.value : 'Farmer');
    }
}
function fillDemo(phone) {
    var e = document.getElementById('txtLoginPhone'), p = document.getElementById('txtLoginPass');
    if (e) e.value = phone;
    if (p) p.value = 'agri1234';
}
function showToast(msg) {
    var t = document.getElementById('toast');
    if (!t) return;
    t.innerHTML = '<svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round"><circle cx="12" cy="12" r="10"/><path d="M12 16v-4"/><path d="M12 8h.01"/></svg><span></span>';
    t.lastChild.textContent = msg;
    t.classList.add('show');
    setTimeout(function () { t.classList.remove('show'); }, 15000);
}
var resendLeft = 0, resendTick = null;
function resendReady() { return resendLeft <= 0; }
function startResendTimer(sec) {
    var link = document.querySelector('#modal-otp .resend'), t = document.getElementById('resendTimer');
    if (!link || !t) return;
    resendLeft = sec;
    if (resendTick) clearInterval(resendTick);
    function paint() {
        if (resendLeft > 0) { link.classList.add('disabled'); t.textContent = ' (available in ' + resendLeft + 's)'; }
        else { link.classList.remove('disabled'); t.textContent = ''; clearInterval(resendTick); }
    }
    paint();
    resendTick = setInterval(function () { resendLeft--; paint(); }, 1000);
}
function mergeOtp() {
    var ins = document.querySelectorAll('#modal-otp input.otp'), v = '';
    for (var i = 0; i < ins.length; i++) v += ins[i].value;
    document.getElementById('hdnOtp').value = v;
    return true;
}
function initOtp() {
    var ins = document.querySelectorAll('#modal-otp input.otp');
    for (var i = 0; i < ins.length; i++) {
        (function (idx) {
            ins[idx].addEventListener('input', function () {
                this.value = this.value.replace(/[^0-9]/g, '').slice(0, 1);
                if (this.value && ins[idx + 1]) ins[idx + 1].focus();
            });
            ins[idx].addEventListener('keydown', function (e) {
                if (e.key === 'Backspace' && !this.value && ins[idx - 1]) ins[idx - 1].focus();
            });
            ins[idx].addEventListener('paste', function (e) {
                var d = (e.clipboardData || window.clipboardData).getData('text').replace(/[^0-9]/g, '');
                if (d.length > 1) {
                    e.preventDefault();
                    for (var k = 0; k < ins.length; k++) ins[k].value = d.charAt(k) || '';
                }
            });
        })(i);
    }
}
document.addEventListener('keydown', function (e) { if (e.key === 'Escape') closeModals(); });
initSegs();
initOtp();