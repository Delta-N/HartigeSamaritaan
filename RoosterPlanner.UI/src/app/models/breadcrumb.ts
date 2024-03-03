export class Breadcrumb {

  constructor(label: string | null, url: string | null) {
    this.label = label;
    this.url = url;
  }

  label: string | null;
  url: string | null
}
