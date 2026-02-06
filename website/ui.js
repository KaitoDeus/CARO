/**
 * UI Component Manager
 * Implements simple OOP structure for rendering reusable components.
 */

class Component {
    constructor(placeholderId) {
        this.placeholderId = placeholderId;
    }

    render() {
        const element = document.getElementById(this.placeholderId);
        if (element) {
            element.innerHTML = this.getTemplate();
        }
    }

    getTemplate() {
        return '';
    }
}

class Header extends Component {
    constructor(placeholderId) {
        super(placeholderId);
        this.navItems = [
            { label: "Tính Năng", link: "features.html" },
            { label: "Hướng Dẫn", link: "guide.html" },
            { label: "Công Nghệ", link: "technology.html" },
            { label: "Về Tác Giả", link: "author.html" }
        ];
    }

    getTemplate() {
        // Detect active link
        const currentPath = window.location.pathname.split("/").pop() || "index.html";
        
        const navLinksHtml = this.navItems.map(item => {
            const isActive = currentPath === item.link ? 'active' : '';
            return `<li><a href="${item.link}" class="${isActive}">${item.label}</a></li>`;
        }).join('');

        return `
        <header>
            <div class="container nav-wrapper">
                <a href="index.html" class="logo">
                    <img src="assets/caro.ico" alt="Logo" style="width: 32px; height: 32px; border-radius: 50%;"> CARO
                </a>
                <nav>
                    <ul class="nav-links">
                        ${navLinksHtml}
                    </ul>
                </nav>
                <a href="https://drive.google.com/drive/folders/1EXDJsEHQUqT0bzCNUvXFhfkYWcwn9d16?usp=sharing" target="_blank" class="btn btn-primary btn-sm">Tải Game Ngay</a>
            </div>
        </header>
        `;
    }
}

class Footer extends Component {
    getTemplate() {
        return `
        <footer>
            <div class="container footer-content">
                <div class="footer-logo">
                    <h3>CARO</h3>
                    <p>Kết nối đam mê - Nâng tầm chiến thuật.</p>
                </div>
                <div class="footer-social">
                    <a href="https://www.facebook.com/kaitovo8952/" target="_blank"><i class="fa-brands fa-facebook"></i> Facebook</a>
                    <a href="https://www.instagram.com/_kai.desu/" target="_blank"><i class="fa-brands fa-instagram"></i> Instagram</a>
                    <a href="mailto:khaivo300605@gmail.com"><i class="fa-solid fa-envelope"></i> Email</a>
                </div>
            </div>
            <div class="copyright">
                <p>&copy; 2025 GameCaro. Developed by Vo Anh Khai.</p>
            </div>
        </footer>
        `;
    }
}

// Initialize Components when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    const header = new Header('app-header');
    header.render();

    const footer = new Footer('app-footer');
    footer.render();
});
