import { CategoryWebsiteDetails } from "../categories/category-website-details.model";

export class WebsiteDetails {
    id :number=0;
    url :string='';
    mainNodeXPathToNewsContainer :string='';
    newsNodeTag :string='';
    newsNodeClass :string='';
    titleNodeTag :string='';
    titleNodeClass :string='';
    descriptionNodeTag :string='';
    descriptionNodeClass :string='';
    imgNodeClass :string='';
    category: string='';
    categories: CategoryWebsiteDetails[] = [];
}
