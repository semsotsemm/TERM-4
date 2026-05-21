import BrandIcon from './BrandIcon'
import TextWithAccent from './TextWithAccent'

const LeftPanel  = () => {
    return (
        <div className="panel-left">
        <BrandIcon />
            <div className="panel-hero">
                <TextWithAccent text='Каталог' accent='товаров'/>
            </div>
        </div>
    )
}

export default LeftPanel