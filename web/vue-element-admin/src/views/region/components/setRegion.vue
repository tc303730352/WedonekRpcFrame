<template>
  <el-dialog
    title="新增机房"
    :visible="visible"
    :close-on-click-modal="false"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="form" :rules="rules" :model="formData" label-width="120px">
      <el-form-item label="机房名" prop="RegionName">
        <el-input v-model="formData.RegionName" maxlength="50" placeholder="机房名" />
      </el-form-item>
      <el-form-item label="所在地区">
        <el-row>
          <el-col :sm="24" :md="6">
            <el-form-item prop="CountryId">
              <el-select v-model="formData.CountryId" clearable placeholder="所在国家" @change="chooseCountry">
                <el-option v-for="item in country" :key="item.Id" :label="item.Name" :value="item.Id" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col v-if="pro.length !=0" :sm="24" :md="6">
            <el-form-item prop="ProId">
              <el-select v-model="formData.ProId" clearable placeholder="所在省份" @change="choosePro">
                <el-option v-for="item in pro" :key="item.Id" :label="item.Name" :value="item.Id" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col v-if="city.length !=0" :sm="24" :md="6">
            <el-form-item prop="CityId">
              <el-select v-model="formData.CityId" clearable placeholder="所在城市" @change="chooseCity">
                <el-option v-for="item in city" :key="item.Id" :label="item.Name" :value="item.Id" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col v-if="district.length !=0" :sm="24" :md="6">
            <el-form-item prop="DistrictId">
              <el-select v-model="formData.DistrictId" clearable placeholder="所在区县">
                <el-option v-for="item in district" :key="item.Id" :label="item.Name" :value="item.Id" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :sm="24" :md="24" style="margin-top:10px">
            <el-form-item prop="Address" style="m">
              <el-input v-model="formData.Address" placeholder="地址" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form-item>
      <el-form-item label="联系人" prop="Contacts">
        <el-input v-model="formData.Contacts" placeholder="联系人" />
      </el-form-item>
      <el-form-item label="联系电话" prop="Phone">
        <el-input v-model="formData.Phone" placeholder="联系电话" />
      </el-form-item>
    </el-form>
    <el-row slot="footer" style="text-align:center;line-height:20px;">
      <el-button type="primary" @click="save">保存</el-button>
      <el-button type="default" @click="reset">重置</el-button>
    </el-row>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import * as regionApi from '@/api/basic/region'
import { LoadArea } from '@/api/area'
export default {
  props: {
    visible: {
      type: Boolean,
      required: true,
      default: false
    },
    id: {
      type: String,
      default: 0
    }
  },
  data() {
    return {
      areas: [],
      country: [],
      pro: [],
      city: [],
      district: [],
      formData: {
        RegionName: null,
        CountryId: null,
        ProId: null,
        CityId: null,
        DistrictId: null,
        Address: null,
        Contacts: null,
        Phone: null
      },
      rules: {
        RegionName: [
          { required: true, message: '机房名不能为空!', trigger: 'blur' }
        ]
      }
    }
  },
  watch: {
    visible: {
      handler(val) {
        if (val) {
          this.reset()
        }
      },
      immediate: true
    }
  },
  mounted() {
    this.loadAreaList()
  },
  methods: {
    moment,
    async loadAreaList() {
      LoadArea().then((res) => {
        this.areas = res
        this.loadCountry()
      })
    },
    loadCountry() {
      this.country = this.areas.filter(c => c.ParentId === 0)
      this.pro = []
      this.city = []
      this.district = []
    },
    chooseCountry() {
      this.pro = this.areas.filter(c => c.ParentId === this.formData.CountryId)
      this.city = []
      this.district = []
      this.formData.CityId = null
      this.formData.ProId = null
      this.formData.DistrictId = null
    },
    choosePro() {
      this.city = this.areas.filter(c => c.ParentId === this.formData.ProId)
      this.district = []
      this.formData.CityId = null
      this.formData.DistrictId = null
    },
    chooseCity() {
      this.district = this.areas.filter(c => c.ParentId === this.formData.CityId)
      this.formData.DistrictId = null
    },
    handleClose() {
      this.$emit('cancel', false)
    },
    async reset() {
      const res = await regionApi.Get(this.id)
      this.formData = {
        RegionName: res.RegionName,
        CountryId: res.CountryId,
        Address: res.Address,
        ProId: null,
        CityId: null,
        DistrictId: null,
        Contacts: res.Contacts,
        Phone: res.Phone
      }
      if (res.CountryId !== 0) {
        this.chooseCountry()
      }
      if (res.ProId !== 0) {
        this.formData.ProId = res.ProId
        this.choosePro()
      }
      if (res.CityId !== 0) {
        this.formData.CityId = res.CityId
        this.chooseCity()
      }
      if (res.DistrictId !== 0) {
        this.formData.DistrictId = res.DistrictId
      }
    },
    save() {
      const that = this
      this.$refs['form'].validate((valid) => {
        if (valid) {
          that.add()
        }
      })
    },
    async add() {
      await regionApi.Set(this.id, this.formData)
      this.$message({
        message: '保存成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    }
  }
}
</script>
