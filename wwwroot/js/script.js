document.addEventListener('DOMContentLoaded', () => {
    const getStartedButton = document.querySelector('.btn.btn-light.btn-lg');
    if (getStartedButton) {
        getStartedButton.addEventListener('click', () => {
            window.location.href = 'MBTI Assessment.html';
        });
    }
    const contactItems = document.querySelectorAll('.contact-item');
    contactItems.forEach(item => {
        item.addEventListener('mouseover', () => {
            item.style.color = 'blue';
        });
        item.addEventListener('mouseout', () => {
            item.style.color = '';
        });
    });
    const homeNowButton = document.querySelector('.btn.btn-light.rounded-pill');
    if (homeNowButton) {
        homeNowButton.addEventListener('click', () => {
            window.scrollTo({ top: 0, behavior: 'smooth' });
        });
    }
    const confirmButton = document.querySelector('.btn-confirm');
    if (confirmButton) {
        confirmButton.addEventListener('click', () => {
            alert('Your answer has been recorded!');
        });
    }
    const seeButtons = document.querySelectorAll('.btn-see');
    seeButtons.forEach(button => {
        button.addEventListener('click', () => {
            alert('Đang di chuyển đến trường mà bạn chọn...');
        });
    });
});
