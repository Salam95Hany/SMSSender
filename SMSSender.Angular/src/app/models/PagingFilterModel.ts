import { FilterModel } from "./FilterModel";

export interface PagingFilterModel {
    pagesize?: number;
    currentpage?: number;
    operationType?: number;
    filterList: FilterModel[];
    userId?: string;
}