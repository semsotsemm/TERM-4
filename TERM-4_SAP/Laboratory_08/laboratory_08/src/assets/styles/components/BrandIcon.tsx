const BrandIcon = () => {
    return(
        <div className="brand">
                <div className="brand-mark">
                <svg viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <rect x="2" y="5" width="16" height="11" rx="1.5" stroke="white" stroke-width="1.5"/>
                    <path d="M7 5V4a3 3 0 016 0v1" stroke="white" stroke-width="1.5" stroke-linecap="round"/>
                    <circle cx="10" cy="10.5" r="1.5" fill="white"/>
                </svg>
                </div>
                <span className="brand-name">ShopLab</span>
            </div>
    )
} 

export default BrandIcon